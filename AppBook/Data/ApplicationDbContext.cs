using Microsoft.EntityFrameworkCore;
using NumePrenume.Models;  // Aici adaugi namespace-ul pentru modelele tale

namespace NumePrenume.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Setul de date pentru entitatea Book
        public DbSet<Book> Books { get; set; }
    }
}
