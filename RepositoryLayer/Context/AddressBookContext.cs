using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {

        }

        public DbSet<AddressBook> AddressBooks { get; set; } = null!;
    }


}
