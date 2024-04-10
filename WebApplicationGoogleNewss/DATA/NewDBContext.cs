using Microsoft.EntityFrameworkCore;

namespace WebApplicationGoogleNewss.DATA
{
    public class NewDBContext : DbContext
    {
        public NewDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Entity.New> News{ get; set; }
    }
}
