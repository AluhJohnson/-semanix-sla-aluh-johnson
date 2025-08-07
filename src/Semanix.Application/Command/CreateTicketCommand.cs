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
    public class CreateTicketCommand : IRequest<Response<Guid>>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PRIORITY Priority { get; set; }
    }
}
