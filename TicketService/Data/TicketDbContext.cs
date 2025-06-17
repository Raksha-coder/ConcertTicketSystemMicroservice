using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TicketService.Model;

namespace TicketService.Data
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options)
        : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketType>()
                .Property(tt => tt.Price)
                .HasPrecision(18, 2);
        }
    }
}
