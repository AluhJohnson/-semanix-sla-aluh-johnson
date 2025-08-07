using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Application.RequestHandler;
using Semanix.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Persistence.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly List<AlertTbl> _alerts = new();
        private readonly IConfiguration _configuration;
        private readonly string? _schema;
        private readonly SemanixDbContext? _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateTicketCommandHandler> _logger;
        private readonly IDbConnection _dbConnection;
        public AlertRepository(IConfiguration configuration, SemanixDbContext? db, IHttpContextAccessor httpContextAccessor, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _schema = _configuration["db.schema"];
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _dbConnection = dbConnection;
        }

        public Task AddAlertAsync(AlertTbl alert)
        {
            //_alerts.Add(alert);
            _db.TicketEvents.Add(alert);
            _db.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task<List<AlertTbl>> GetAlertsForTenantAsync(string tenantId)
        {
            var result = _db.TicketEvents.Where(a => a.TenantId == tenantId).ToList();
            //var result = _alerts.Where(a => a.TenantId == tenantId).ToList();
            return Task.FromResult(result);
        }
    }
}
