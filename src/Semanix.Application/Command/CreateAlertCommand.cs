using MediatR;
using Semanix.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.Command
{
    public class CreateAlertCommand : IRequest<Guid>
    {
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public Guid TicketId { get; set; }
        public string Type { get; set; } // "Warning" or "Breach"
    }
}
