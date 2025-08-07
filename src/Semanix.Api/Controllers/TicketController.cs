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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketService;
        private readonly IMediator _mediator;
        
        public TicketController(ITicketRepository ticketService, IMediator mediator)
        {
            _ticketService = ticketService;
            _mediator = mediator;
        }

        [HttpPost("add-ticket")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddTicketAsync([FromBody] CreateTicketCommand createTicketCommand)
        {        
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            var result = await _mediator.Send(createTicketCommand);
            return StatusCode((int)HttpStatusCode.OK, result);
            //return StatusCode(HttpStatusCode.OK, result);
        }

        [HttpGet("get-ticket-by-id")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetTicketByTenantidAsync([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            var result = await _mediator.Send(new GetTicketsByTenantQuery { TenantId = id});
            return StatusCode((int)HttpStatusCode.OK, result);
            //return StatusCode(HttpStatusCode.OK, result);
        }

        [HttpPost("update-ticket-by-id")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTicketByTenantidAsync([FromBody] ChangeTicketStatusCommand changeTicketStatusCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            var result = await _mediator.Send(changeTicketStatusCommand);
            return StatusCode((int)HttpStatusCode.OK, result);
            //return StatusCode(HttpStatusCode.OK, result);
        }
    }
}
