using Microsoft.EntityFrameworkCore;
using Planner.DAL.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Planner.DAL
{
    public class PlannerDbContext : DbContext
    {
        public PlannerDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Stage> Stages { get; set; }
    }
}
