using ContactAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Data
{
    public class ContactAPIDBContext : DbContext
    {
        public ContactAPIDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
