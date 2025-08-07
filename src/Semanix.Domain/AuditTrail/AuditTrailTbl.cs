using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Semanix.Domain.AuditTrail;

[Table("AuditTrailTbl", Schema = "Semanix")]
public class AuditTrailTbl
{
    [Key]
    public long Id { get; set; }
    public string? ActionName { get; set; }
    public string? ActionDescription { get; set; }
    public string? Module { get; set; }
    public string? UserName { get; set; }
    public string? StaffId { get; set; }
    public string? SolId { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public DateTime ActionTime { get; set; }
    public string? Origin { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime LastModified { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public string? MachineName { get; set; }
}