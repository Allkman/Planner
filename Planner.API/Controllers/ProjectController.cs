using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Planner.DAL;
using Planner.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.QueryFilters;

namespace Planner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly PlannerDbContext _dbContext;

        public ProjectController(ILogger logger, PlannerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _dbContext.Projects.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet]
        [Route("/api/projects/{pid:int}/tickets")]
        public async Task<IActionResult> GetProjectTickets(int pId,
            [FromQuery] ProjectStageQueryFilter filter)
        {
            IQueryable<Stage> stages = _dbContext.Stages.Where(t => t.ProjectId == pId);
            if (filter != null && !string.IsNullOrWhiteSpace(filter.Owner))
                stages = stages.Where(t => !string.IsNullOrWhiteSpace(t.Owner) &&
                    t.Owner.ToLower() == filter.Owner.ToLower());

            var listStages = await stages.ToListAsync();
            if (listStages == null || listStages.Count <= 0)
                return NotFound();

            return Ok(listStages);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Project project)
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                    new { id = project.ProjectId },
                    project
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Project project)
        {
            if (id != project.ProjectId) return BadRequest();

            _dbContext.Entry(project).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                if (_dbContext.Projects.Find(id) == null)
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            if (project == null) return NotFound();

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return Ok(project);
        }
    }
}
