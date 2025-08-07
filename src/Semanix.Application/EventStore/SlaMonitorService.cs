using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Prometheus;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Common.Enums;
using Metrics = Prometheus.Metrics;

namespace Semanix.Application.EventStore
{
    public class SlaMonitorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SlaMonitorService> _logger;
        // Define the counter
        private static readonly Counter SlaBreachedCounter = Metrics.CreateCounter("tickets_sla_breached_total", "Total number of tickets that breached SLA");

        public SlaMonitorService(IServiceProvider serviceProvider, ILogger<SlaMonitorService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ISemanixDbContext>();
                var alertService = scope.ServiceProvider.GetRequiredService<IAlertRepository>();

                var now = DateTime.UtcNow;
                var soon = now.AddSeconds(10);
                //var soon = now.AddMinutes(10);

                var tickets = context.Tickets.Where(t => t.Status != STATUS.Closed && t.Status != STATUS.Breached).ToList();
                foreach (var ticket in tickets)
                {
                    if (ticket.SlaDeadlineUtc <= now)
                    {
                        ticket.Status = STATUS.Breached;
                        await alertService.AddAlertAsync(new Domain.AlertTbl
                        {
                            EntityId = ticket.EntityId,
                            TenantId = ticket.TenantId,
                            TicketId=ticket.Id,
                            TimestampUtc = DateTime.UtcNow,
                            Type = "Breach"
                        });
                        SlaBreachedCounter.Inc(); // ✅ Increment the breach counter
                        _logger.LogWarning("SLA Breach for Ticket {Id}", ticket.Id);
                    }
                    else if (ticket.SlaDeadlineUtc <= soon)
                    {
                        await alertService.AddAlertAsync(new Domain.AlertTbl
                        {
                            EntityId = ticket.EntityId,
                            TenantId = ticket.TenantId,
                            TicketId = ticket.Id,
                            TimestampUtc = DateTime.UtcNow,
                            Type = "Warning"
                        });

                        _logger.LogInformation("SLA Warning for Ticket {Id}", ticket.Id);
                    }
                }

                await context.SaveChangesAsync(stoppingToken);
                //await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

}
