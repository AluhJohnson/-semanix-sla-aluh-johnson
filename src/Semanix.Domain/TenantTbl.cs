using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Semanix.Domain;

[Table("TenantTbl", Schema = "Semanix")]
public class TenantTbl //: Entity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
}