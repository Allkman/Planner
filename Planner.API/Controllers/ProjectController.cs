using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.API.DTOs;
using Planner.API.QueryFilters;
using Planner.DAL;
using Planner.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly PlannerDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProjectController(PlannerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProjectDTO>> Get()
        {
            var entityList = await _dbContext.Projects.ToListAsync();
            var model = _mapper.Map<IEnumerable<ProjectDTO>>(entityList);
            return Ok(model);
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDTO>> GetById(int id)
        {
            var entity = await _dbContext.Projects.Where(x => x.ProjectId == id).FirstOrDefaultAsync();
            if (entity == null) return NotFound();     
            var model = _mapper.Map<ProjectDTO>(entity);
       
            return Ok(model);
        }
        /// <summary>
        /// Filter Tickets by Owner in one particular Project
        /// </summary>
        [HttpGet]
        [Route("/api/projects/{id:int}/tickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDTO>> GetProjectTickets(int id,
            [FromQuery] ProjectTicketQueryFilter filter)
        {
            IQueryable<Ticket> tickets = _dbContext.Tickets.Where(t => t.ProjectId == id);
            if (filter != null && !string.IsNullOrWhiteSpace(filter.Owner))
                tickets = tickets.Where(t => !string.IsNullOrWhiteSpace(t.Owner) &&
                    t.Owner.ToLower() == filter.Owner.ToLower());

            var listTickets = await tickets.ToListAsync();
            if (listTickets == null) return NotFound();

            var model = _mapper.Map<IEnumerable<TicketDTO>>(listTickets);

            return Ok(model);            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProjectDTO>> Post([FromBody] ProjectDTO projectDto)
        {
            var model = _mapper.Map<Project>(projectDto);
            _dbContext.Projects.Add(model);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                    new { id = model.ProjectId },
                    model
                );           
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectDTO>> Put([FromRoute] int id, [FromBody] ProjectDTO projectDto)
        {
            if (projectDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest("Invalid model object");

            var entity = _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
            if (entity == null)
                return NotFound();

            await _mapper.Map(projectDto, entity);
            _dbContext.Entry(projectDto).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDTO>> Delete(int id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            if (project == null) return NotFound();
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return Ok(project);
        }
    }
}
