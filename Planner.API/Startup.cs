using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Planner.DAL;
using Planner.DAL.FluentValidations;
using Planner.DAL.Models;
using System;
using Microsoft.EntityFrameworkCore;
using MicroElements.Swashbuckle.FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using Planner.API.AutoMapper;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Planner.API.Services;
using System.Reflection;
using System.IO;

namespace Planner.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
    
            services.AddDbContext<PlannerDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionString:PlannerDb"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration.GetValue<string>("Account:Issuer"),
                ValidAudience = Configuration.GetValue<string>("Account:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Account:Secret"))),
                NameClaimType = ClaimTypes.NameIdentifier,
            };

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = tokenValidationParameters;
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
            });
            services.AddTransient<IAccountService, AccountService>();
            services.Configure<AccountOptions>(Configuration.GetSection("Account"));
            services.AddMvc(setup =>
            {
                //...mvc setup...
            }).AddFluentValidation();
            services.AddTransient<IValidator<Ticket>, TicketValidator>();

            // HttpContextServiceProviderValidatorFactory requires access to HttpContext
            services.AddHttpContextAccessor();
            services.ConfigureAutoMapper();

            services.AddControllers()
                .AddMvcOptions(opt =>
                {
                    opt.ReturnHttpNotAcceptable = true;
                })
            // Adds fluent validators to Asp.net
            .AddFluentValidation(c =>
            {
                c.RegisterValidatorsFromAssemblyContaining<Startup>();
                // Optionally set validator factory if you have problems with scope resolve inside validators.
                c.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
            });
            services.AddAuthorization();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Planner.API",
                    Version = "v1",
                    Description = "Project planing application",
                    Contact = new OpenApiContact
                    {
                        Name = "Algirdas Cernevicius",
                        Email = string.Empty,
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT Licence",
                        Url = new Uri("https://opensource.org/licenses/mit"),
                    }                   
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securitySchema, new[] { "Bearer" }
                    }
                });
                c.OperationFilter<AssignContentTypeFilter>();
            });
            // Adds FluentValidationRules staff to Swagger. (Minimal configuration)
            services.AddFluentValidationRulesToSwagger();
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                   builder => builder
                   .WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Planner.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
