using MediatR;
using Semanix.Application.Ticket.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.Query
{
    public class GetTicketsByTenant : IRequest<List<CreateTicketDto>>
    {
        public string TenantId { get; set; } = default!;
    }
}
