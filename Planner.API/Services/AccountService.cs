using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Planner.API.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Planner.API.Services
{

    public class AccountService : IAccountService
    {
        private readonly AccountOptions _options;

        public AccountService(IOptions<AccountOptions> options)
        {
            _options = options.Value;
        }

        public LoginResultDTO Authenticate(LoginRequestDTO request)
        {
            var user = new {
                userName = "algirdas",
                password = "123asd",
                id = 1,
                firstName = "Algirdas",
                lastName = "Cernevicius" 
            };
            if (request.UserName == user.userName && request.Password == user.password)
            {
                var model = new LoginResultDTO
                {
                    Id = user.id,
                    FirstName = user.firstName,
                    LastName = user.lastName,
                    UserName = user.userName,
                };
                model.Token = GenerateTwtToken(model);
                return model;
            }
            return null;
        }

        private string GenerateTwtToken(LoginResultDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(_options.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
