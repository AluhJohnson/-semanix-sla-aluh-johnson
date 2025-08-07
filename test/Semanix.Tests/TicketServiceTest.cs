using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Semanix.Application.Command;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Application.Query;
using Semanix.Application.Ticket.Dto;
using Semanix.Common.Enums;
using Semanix.Domain;
using SlipFree.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Semanix.Tests;

[TestFixture]
public class TicketServiceTest
{
    private Mock<ITicketRepository>? _tickettServiceMock;
    private Mock<IMediator> _mediatorMock = null!;
    private TicketController? _ticketController;

    [SetUp]
    public void Setup()
    {
        _tickettServiceMock = new Mock<ITicketRepository>();
        _mediatorMock = new Mock<IMediator>();
        _ticketController = new TicketController(_tickettServiceMock.Object, _mediatorMock.Object);
    }
    
    [Test]
    public async Task Add_Ticket_Async()
    {
        Guid tenant = Guid.NewGuid();
        CancellationToken token = new CancellationToken();

        var response = new Guid(); // expected response (should match controller's logic)

        _tickettServiceMock!.Setup(x => x.AddTicketAsync(new CreateTicketCommand
        {

        }, token)).ReturnsAsync(tenant);

        _ticketController = new TicketController(_tickettServiceMock.Object, _mediatorMock.Object);
        
        //Act
        var result = await _ticketController.AddTicketAsync(new CreateTicketCommand
        {

        });
        var values = result as ObjectResult;
        // Assert
        Assert.AreEqual(StatusCodes.Status200OK, values?.StatusCode);
        Assert.IsNotNull(values);
        Assert.AreEqual(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(values?.Value));
    }

    [Test]
    public async Task Get_Ticket_By_Tenant_idAsync()
    {
        Guid tenant = Guid.NewGuid();
        CancellationToken token = new CancellationToken();

        var response = new List<CreateTicketDto> { }; // expected response (should match controller's logic)

        _tickettServiceMock!.Setup(x => x.GetTicketsByTenant(new GetTicketsByTenant { TenantId="123455"})).ReturnsAsync(response);

        _ticketController = new TicketController(_tickettServiceMock.Object, _mediatorMock.Object);

        //Act
        var result = await _ticketController.GetTicketByTenantidAsync("123455");
        var values = result as ObjectResult;
        // Assert
        Assert.AreEqual(StatusCodes.Status200OK, values?.StatusCode);
        Assert.IsNotNull(values);
        Assert.AreEqual(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(values?.Value));
    }

    [Test]
    public async Task Update_Ticket_Status()
    {
        Guid tenant = Guid.NewGuid();
        CancellationToken token = new CancellationToken();

        var response = new TicketTbl { }; // expected response (should match controller's logic)

        _tickettServiceMock!.Setup(x => x.UpdateTicketStatus(new ChangeTicketStatusCommand { NewStatus = STATUS.Open, TicketId=Guid.NewGuid() }, token)).ReturnsAsync(response);

        _ticketController = new TicketController(_tickettServiceMock.Object, _mediatorMock.Object);

        //Act
        var result = await _ticketController.GetTicketByTenantidAsync("123455");
        var values = result as ObjectResult;
        // Assert
        Assert.AreEqual(StatusCodes.Status200OK, values?.StatusCode);
        Assert.IsNotNull(values);
        Assert.AreEqual(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(values?.Value));
    }
}