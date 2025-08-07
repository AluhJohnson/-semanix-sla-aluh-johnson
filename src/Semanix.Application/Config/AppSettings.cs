using System.ComponentModel.DataAnnotations;

namespace Semanix.Application.Config;

public class AppSettings
{
    public string XToken { get; set; } = string.Empty;
    public string XEntityId { get; set; } = string.Empty;
    [Required]
    public string XTenantId { get; set; } = string.Empty;
}