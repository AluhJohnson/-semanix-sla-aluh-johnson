namespace Semanix.Application.Response;

public class SemanixApiBaseResponse<T>
{
    public bool IsSuccessful => Code.Equals("00");
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public T? Data { get; set; } 
}