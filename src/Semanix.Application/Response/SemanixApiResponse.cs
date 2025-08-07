namespace Semanix.Application.Response;

public class SemanixApiResponse<T>
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<T>? Data { get; set; } 
}