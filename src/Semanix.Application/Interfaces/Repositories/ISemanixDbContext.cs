using Semanix.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.Interfaces.Repositories
{
    public interface ISemanixDbContext
    {
        public IQueryable<TenantTbl> Tenants { get; }
        public IQueryable<TicketTbl> Tickets { get; }
        public IQueryable<AlertTbl> TicketEvents { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
