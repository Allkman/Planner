using Planner.DAL.Models;
using System;

namespace Planner.API.DTOs
{
    public class TicketDTO
    {
        public int? TicketId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Project Project { get; set; }
    }
}
