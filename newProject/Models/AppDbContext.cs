using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore;

namespace newProject.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationClass>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
    
}
