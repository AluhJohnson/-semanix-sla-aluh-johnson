using MediatR;
using Semanix.Application.Ticket.Dto;
using Semanix.Common.Generic;
namespace Semanix.Application.Query
{
    public class GetTicketEventsByTenantQuery : IRequest<Response<List<object>>>
    {
        public string TenantId { get; set; } = default!;
    }

    public class GetTicketsByTenantQuery : IRequest<Response<List<CreateTicketDto>>>
    {
        public string TenantId { get; set; } = default!;
    }
}

