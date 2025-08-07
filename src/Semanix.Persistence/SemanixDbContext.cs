using Microsoft.EntityFrameworkCore;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Domain;

namespace Semanix.Persistence
{
    public class SemanixDbContext : DbContext, ISemanixDbContext
    {
        public DbSet<TenantTbl> Tenants { get; set; } = null!;
        public DbSet<TicketTbl> Tickets { get; set; } = null!;
        public DbSet<AlertTbl> TicketEvents { get; set; } = null!;

        IQueryable<TenantTbl> ISemanixDbContext.Tenants => Tenants;
        IQueryable<TicketTbl> ISemanixDbContext.Tickets => Tickets;
        IQueryable<AlertTbl> ISemanixDbContext.TicketEvents => TicketEvents;

        public SemanixDbContext(DbContextOptions<SemanixDbContext> options)
       : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer("SemanixDbContext");
    }
}
