using Microsoft.EntityFrameworkCore;

namespace geoDistance.Models
{
    public class AddressContext : DbContext
    {
        public DbSet<Address> addresses { get; set; }

        public AddressContext(DbContextOptions<AddressContext> options) : base(options)
        {

        }
    }
}