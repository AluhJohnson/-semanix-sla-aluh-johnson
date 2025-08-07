using MediatR;
using Semanix.Application.Command;
using Semanix.Application.Query;
using Semanix.Application.Ticket.Dto;
using Semanix.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.Interfaces.Repositories
{
    public interface ITicketRepository
    {
        Task<Guid> AddTicketAsync(CreateTicketCommand tkt, CancellationToken cancellationToken);
        Task<List<CreateTicketDto>> GetTicketsByTenant(GetTicketsByTenant tkt);
        Task<TicketTbl> UpdateTicketStatus(ChangeTicketStatusCommand tkt, CancellationToken cancellationToken);

    }
}
