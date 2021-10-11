using System.Collections.Generic;

namespace Planner.DAL.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
