using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
    
}
