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
    public class StageController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly PlannerDbContext _dbContext;

        public StageController(ILogger logger ,PlannerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] StageQueryFilter ticketQueryFilter)
        {
            IQueryable<Stage> tickets = _dbContext.Stages;

            if (ticketQueryFilter != null)
            {
                if (ticketQueryFilter.Id.HasValue)
                    tickets = tickets.Where(x => x.StageId == ticketQueryFilter.Id);

                if (!string.IsNullOrWhiteSpace(ticketQueryFilter.TitleOrDescription))
                    tickets = tickets.Where(x => x.Title.Contains(ticketQueryFilter.TitleOrDescription,
                       StringComparison.OrdinalIgnoreCase) ||
                       x.Description.Contains(ticketQueryFilter.TitleOrDescription,
                       StringComparison.OrdinalIgnoreCase));
            }
            return Ok(await tickets.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _dbContext.Stages.FindAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPost]
        //[Stage_EnsureDescriptionPresentActionFilter]
        public async Task<IActionResult> Post([FromBody] Stage stage)
        {
            _dbContext.Stages.Add(stage);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                    new { id = stage.StageId },
                    stage
                );
        }

        [HttpPut("{id}")]
        //[Ticket_EnsureDescriptionPresentActionFilter]
        public async Task<IActionResult> Put(int id, [FromBody] Stage stage)
        {
            if (id != stage.StageId) return BadRequest();

            _dbContext.Entry(stage).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                if (await _dbContext.Stages.FindAsync(id) == null)
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stage = await _dbContext.Stages.FindAsync(id);
            if (stage == null) return NotFound();

            _dbContext.Stages.Remove(stage);
            await _dbContext.SaveChangesAsync();

            return Ok(stage);
        }
    }
}
