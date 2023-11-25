using Microsoft.EntityFrameworkCore;
using MVC_EF_Start.Models;

namespace MVC_EF_Start.DataAccess
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Quote> Quotes { get; set; }

    public DbSet<Student> students { get; set; }
        public DbSet<ProjectDocument> Projects { get; set; }
        public DbSet<DownloadInformation> Downloads { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Stations> Fuel_Stations { get; set; }
    }
}