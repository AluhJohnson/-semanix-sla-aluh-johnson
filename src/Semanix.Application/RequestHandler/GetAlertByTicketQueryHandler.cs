using MediatR;
using Microsoft.Extensions.Logging;
using Semanix.Application.BaseHelpers;
using Semanix.Application.Query;
using Semanix.Application.Ticket.Dto;
using Semanix.Common.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.RequestHandler
{
    public class GetAlertByTicketQueryHandler : IRequestHandler<GetTicketEventsByTenantQuery, Response<List<object>>>
    {
        private readonly HttpContextServiceClient _httpContextServiceClient;

        private readonly ILogger<CreateTicketCommandHandler> _logger;

        public GetAlertByTicketQueryHandler(HttpContextServiceClient httpContextServiceClient)
        {
            _httpContextServiceClient = httpContextServiceClient;
        }


        public async Task<Response<List<object>>> Handle(GetTicketEventsByTenantQuery request, CancellationToken cancellationToken)
        {
            var res = await _httpContextServiceClient.alertRepository.GetAlertsForTenantAsync(request.TenantId);
            var alerts = new List<object>{};
            foreach (var item in res)
            {
                var alert = new
                {
                    TenantId = item.TenantId,
                    TicketId = item.TicketId,
                    EntityId = item.EntityId,
                    Type = item.Type,
                    TimestampUtc = item.TimestampUtc,
                };
                alerts.Add(alert);
            }

            return new Response<List<object>> { Data = alerts, Code = res.Count() > 0 ? "99" : "00", Message = string.IsNullOrWhiteSpace($"{res}") ? "No record found" : $"{res.Count()} records found" };

        }
    }
}
