using MediatR;
using Semanix.Common.Enums;
using Semanix.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.Command
{
    public class ChangeTicketStatusCommand : IRequest<Response<object>>
    {
        public Guid TicketId { get; set; }
        public STATUS NewStatus { get; set; }
    }
}
