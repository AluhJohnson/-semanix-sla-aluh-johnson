using Azure.Core;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Semanix.Application.Command;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Application.Query;
using Semanix.Application.RequestHandler;
using Semanix.Application.Ticket.Dto;
using Semanix.Application.Utilities;
using Semanix.Common.Enums;
using Semanix.Domain;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Threading;

namespace Semanix.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string? _schema;
        private readonly SemanixDbContext? _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateTicketCommandHandler> _logger;
        private readonly IDbConnection _dbConnection;
        public TicketRepository(IConfiguration configuration, SemanixDbContext? db, IHttpContextAccessor httpContextAccessor, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _schema = _configuration["db.schema"];
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _dbConnection = dbConnection;
        }
        public async Task<Guid> AddTicketAsync(CreateTicketCommand tkt, CancellationToken cancellationToken)
        {
            var entityId = _httpContextAccessor.HttpContext?.Request.Headers["X-Entity-Id"].FirstOrDefault();
            var now = DateTime.UtcNow;

            var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(tenantId))
                throw new ValidationException("X-Tenant-Id header is required.");

            var ticket = new TicketTbl
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                EntityId = entityId,
                Title = tkt.Title,
                Description = tkt.Description,
                Priority = tkt.Priority,
                CreatedUtc = now,
                SlaDeadlineUtc = SlaCalculator.CalculateDeadline(now, tkt.Priority),
                Status = STATUS.Open
            };
            _db.Tickets.Add(ticket);
            await _db.SaveChangesAsync(cancellationToken);
            return ticket.Id;
        }

        public async Task<List<CreateTicketDto>> GetTicketsByTenant(GetTicketsByTenant tkt)
        {
            var sql = @"SELECT Id, Title, Priority, Status, CreatedUtc, SlaDeadlineUtc
                    FROM TicketTbls
                    WHERE TenantId = @TenantId";

            _dbConnection.ConnectionString = _db.Database.GetConnectionString();

            var tickets = await _dbConnection.QueryAsync<CreateTicketDto>(sql, new { tkt.TenantId });
            return tickets.ToList();
        }

        //public async Task<Unit> UpdateTicketStatus(ChangeTicketStatusCommand tkt, CancellationToken cancellationToken)
        public async Task<TicketTbl> UpdateTicketStatus(ChangeTicketStatusCommand tkt, CancellationToken cancellationToken)
        {
            var ticket = await _db.Tickets.FindAsync([tkt.TicketId], cancellationToken);

            if (ticket == null)
                throw new ValidationException($"No ticket found");

            if (!IsValidTransition(ticket.Status, tkt.NewStatus))
                throw new ValidationException($"Invalid status transition: {ticket.Status} → {tkt.NewStatus}");

            ticket.Status = tkt.NewStatus;
            ticket.LastStatusChangeUtc = DateTime.UtcNow;

            // Reset SLA if reopened
            if (tkt.NewStatus == STATUS.InProgress && ticket.Status == STATUS.Resolved)
            {
                ticket.SlaDeadlineUtc = SlaCalculator.CalculateDeadline(DateTime.UtcNow, ticket.Priority);
            }

            await _db.SaveChangesAsync(cancellationToken);
            return ticket;
            //return Unit.Value;
        }

        private bool IsValidTransition(STATUS current, STATUS next) =>
        (current == STATUS.Open && (next == STATUS.InProgress || next == STATUS.Closed)) ||
        (current == STATUS.InProgress && next == STATUS.Resolved) ||
        (current == STATUS.Resolved && (next == STATUS.Closed || next == STATUS.InProgress));
    }
}
