using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
namespace WebAPI.DataAccess
{
    public class WebAPIContext : DbContext
    {
        public WebAPIContext(DbContextOptions<WebAPIContext> options) : base(options) {}

        public DbSet<StudentData> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentData>(entity =>
            {
                entity.HasKey(e => e.StudentId);
            });
        }
    }
}
