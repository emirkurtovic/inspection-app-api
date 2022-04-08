using Microsoft.EntityFrameworkCore;
using WebAPI1web.Models;

namespace WebAPI1web.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<InspectionType> InspectionTypes { get; set; }
    }
}
