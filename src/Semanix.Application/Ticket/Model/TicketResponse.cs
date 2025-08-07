namespace Semanix.Application.Ticket.Model;

public class TicketResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string? TokenType { get; set; }
}