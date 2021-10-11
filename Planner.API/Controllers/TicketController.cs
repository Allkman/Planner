using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class TicketController : ControllerBase
    {
        private readonly PlannerDbContext _dbContext;
        private readonly IMapper _mapper;

        public TicketController(PlannerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TicketDTO>> Get([FromQuery] TicketQueryFilter ticketQueryFilter)
        {
            IQueryable<Ticket> tickets = _dbContext.Tickets;

            if (ticketQueryFilter != null)
            {
                if (ticketQueryFilter.Id.HasValue)
                    tickets = tickets.Where(x => x.TicketId == ticketQueryFilter.Id);

                if (!string.IsNullOrWhiteSpace(ticketQueryFilter.TitleOrDescription))
                    tickets = tickets.Where(x => x.Title.Contains(ticketQueryFilter.TitleOrDescription,
                       StringComparison.OrdinalIgnoreCase) ||
                       x.Description.Contains(ticketQueryFilter.TitleOrDescription,
                       StringComparison.OrdinalIgnoreCase));
            }

            var model = _mapper.Map<IEnumerable<TicketDTO>>(tickets);
            return Ok(model);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketDTO>> GetById(int id)
        {
            var entity = await _dbContext.Tickets.FindAsync(id);
            if (entity == null) return NotFound();

            var model = _mapper.Map<TicketDTO>(entity);

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TicketDTO>> Post([FromBody] TicketDTO ticketDto)
        {
            var model = _mapper.Map<Ticket>(ticketDto);

            _dbContext.Tickets.Add(model);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                    new { id = ticketDto.TicketId },
                    ticketDto
                );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TicketDTO>> Put(int id, [FromBody] TicketDTO ticketDto)
        {
            if (id != ticketDto.TicketId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest("Invalid model object");
            var entity = _dbContext.Tickets.FirstOrDefaultAsync(x => x.TicketId == id);
            if (entity == null) return NotFound();

            await _mapper.Map(ticketDto, entity);
            _dbContext.Entry(ticketDto).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketDTO>> Delete(int id)
        {
            var ticket = await _dbContext.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            _dbContext.Tickets.Remove(ticket);
            await _dbContext.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}
