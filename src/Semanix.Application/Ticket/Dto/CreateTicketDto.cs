using Semanix.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Semanix.Application.Ticket.Dto;

public class CreateTicketDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public PRIORITY Priority { get; set; }
    public STATUS Status { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime SlaDeadlineUtc { get; set; }
}