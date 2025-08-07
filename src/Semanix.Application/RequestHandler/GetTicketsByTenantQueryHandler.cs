using MediatR;
using Microsoft.Extensions.Logging;
using Semanix.Application.BaseHelpers;
using Semanix.Application.Query;
using Semanix.Application.Ticket.Dto;
using Semanix.Common.Generic;

namespace Semanix.Application.RequestHandler
{
    public class GetTicketsByTenantQueryHandler : IRequestHandler<GetTicketsByTenantQuery, Response<List<CreateTicketDto>>>
    {
        private readonly HttpContextServiceClient _httpContextServiceClient;

        private readonly ILogger<CreateTicketCommandHandler> _logger;

        public GetTicketsByTenantQueryHandler(HttpContextServiceClient httpContextServiceClient)
        {
            _httpContextServiceClient = httpContextServiceClient;
        }


        public async Task<Response<List<CreateTicketDto>>> Handle(GetTicketsByTenantQuery request, CancellationToken cancellationToken)
        {
            var res = await _httpContextServiceClient.ticketRepository.GetTicketsByTenant(new GetTicketsByTenant { TenantId = request.TenantId});

            return new Response<List<CreateTicketDto>> { Data = res, Code = res.Count() > 0 ? "99" : "00", Message = string.IsNullOrWhiteSpace($"{res}") ? "No record found" : $"{res.Count()} records found" };

        }
    }
}
