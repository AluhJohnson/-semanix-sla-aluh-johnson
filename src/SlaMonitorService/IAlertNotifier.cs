using Semanix.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SlaMonitorService
{
    public class HttpAlertNotifier : IAlertNotifier
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpAlertNotifier(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendAlertAsync(TicketTbl ticket, string type)
        {
            var alert = new
            {
                TicketId = ticket.Id,
                TenantId = ticket.TenantId,
                EntityId = ticket.EntityId,
                Type = type,
                TimestampUtc = DateTime.UtcNow
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{_config["AlertService:BaseUrl"]}/internal/alerts", alert);

            response.EnsureSuccessStatusCode();
        }
    }

    public interface IAlertNotifier
    {
    }
}
