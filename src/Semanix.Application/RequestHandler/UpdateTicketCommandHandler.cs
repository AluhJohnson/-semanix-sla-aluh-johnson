using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Semanix.Application.BaseHelpers;
using Semanix.Application.Command;
using Semanix.Application.Utilities;
using Semanix.Common.Enums;
using Semanix.Common.Generic;
using Semanix.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.RequestHandler
{
    public class UpdateTicketCommandHandler : IRequestHandler<ChangeTicketStatusCommand, Response<object>>
    {
        private readonly HttpContextServiceClient _httpContextServiceClient;

        private readonly ILogger<CreateTicketCommandHandler> _logger;

        public UpdateTicketCommandHandler(HttpContextServiceClient httpContextServiceClient)
        {
            _httpContextServiceClient = httpContextServiceClient;
        }

        public async Task<Response<object>> Handle(ChangeTicketStatusCommand request, CancellationToken cancellationToken)
        {
            var res = await _httpContextServiceClient.ticketRepository.UpdateTicketStatus(request, cancellationToken);

            if (res != null)
            {
                var r = new
                {
                    Title=res.Title,
                    Status = res.Status.ToString(),
                    Priority = res.Priority.ToString(),
                    TenantId = res.TenantId.ToString(),
                    Id = res.Id,
                };

                return new Response<object> { Data = r, Code = "00", Message = "successful" };
            }

            return new Response<object> { Code = "99", Message= "Request failed" };
        }
    }
}
