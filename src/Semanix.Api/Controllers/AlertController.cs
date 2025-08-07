using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Semanix.Application.Command;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Application.Query;
using Semanix.Application.Response;
using System.Net;

namespace SlipFree.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AlertController : ControllerBase
    {
        private readonly IAlertRepository _alertService;
        private readonly IMediator _mediator;
        
        public AlertController(IAlertRepository alertService, IMediator mediator)
        {
            _alertService = alertService;
            _mediator = mediator;
        }

        [HttpGet("get-alert-by-id")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAlertByTenantidAsync([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            var result = await _mediator.Send(new GetTicketEventsByTenantQuery { TenantId = id });
            return StatusCode((int)HttpStatusCode.OK, result);
        }

    }
}
