using Microsoft.AspNetCore.Http;
using Semanix.Application.Interfaces.Repositories;

namespace Semanix.Application.BaseHelpers;

public class HttpContextServiceClient
{
    public HttpContextServiceClient(ITicketRepository ticketRepository, IAlertRepository? alertRepository, IHttpContextAccessor contextAccessor)
    {
        this.ticketRepository = ticketRepository;
        this.alertRepository = alertRepository;
        this.contextAccessor = contextAccessor;
    }

    // Public properties for encapsulation
    public IHttpContextAccessor? contextAccessor { get; }
    public ITicketRepository? ticketRepository { get; set; }
    public IAlertRepository? alertRepository { get; set; }
}