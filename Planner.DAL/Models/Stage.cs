using System;
using System.ComponentModel.DataAnnotations;

namespace Planner.DAL.Models
{
    public class Stage
    {
        [Key]
        public int? StageId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Project Project { get; set; }
    }
}
