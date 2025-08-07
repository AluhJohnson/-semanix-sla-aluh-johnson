using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Domain
{
    public class AlertTbl
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public Guid TicketId { get; set; }
        public string Type { get; set; } // "Warning" or "Breach"
        public DateTime TimestampUtc { get; set; }
    }
}
