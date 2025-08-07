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
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Response<Guid>>
    {
        private readonly HttpContextServiceClient _httpContextServiceClient;

        private readonly ILogger<CreateTicketCommandHandler> _logger;

        public CreateTicketCommandHandler(HttpContextServiceClient httpContextServiceClient)
        {
            _httpContextServiceClient = httpContextServiceClient;
        }

        public async Task<Response<Guid>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var res = await _httpContextServiceClient.ticketRepository.AddTicketAsync(request, cancellationToken);

            return new Response<Guid> { Data= res, Code = string.IsNullOrWhiteSpace($"{res}") ? "99" : "00", Message= string.IsNullOrWhiteSpace($"{res}") ? "Request failed" : "successful" };
        }
    }
}
