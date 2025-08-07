using Semanix.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Semanix.Domain;

//[Table("TicketTbl", Schema = "Semanix")]
//[Table("TicketTbl", Schema = "Semanix")]
public class TicketTbl //: Entity
{
    [Key]
    public Guid Id { get; set; }
    public string TenantId { get; set; } = default!;
    public string? EntityId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public PRIORITY Priority { get; set; }
    public STATUS Status { get; set; } = STATUS.Open;
    public DateTime CreatedUtc { get; set; }
    public DateTime SlaDeadlineUtc { get; set; }
    public DateTime? LastStatusChangeUtc { get; set; }
}