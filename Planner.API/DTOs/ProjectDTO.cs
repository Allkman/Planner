using Planner.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planner.API.DTOs
{
    public class ProjectDTO
    {
        public int? ProjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string ClientName { get; set; }
        public List<Ticket> Tickets { get; set; }


    }
}
