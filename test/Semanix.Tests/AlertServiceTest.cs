using Microsoft.AspNetCore.Http;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Domain;
using SlipFree.Api.Controllers;

namespace Semanix.Tests;

[TestFixture]
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Newtonsoft.Json;

[TestFixture]
public class AlertServiceTest
{
    private Mock<IAlertRepository> _alertServiceMock = null!;
    private Mock<IMediator> _mediatorMock = null!;
    private AlertController _alertController = null!;

    [SetUp]
    public void Setup()
    {
        _alertServiceMock = new Mock<IAlertRepository>();
        _mediatorMock = new Mock<IMediator>();
        _alertController = new AlertController(_alertServiceMock.Object, _mediatorMock.Object);
    }

    [Test]
    public async Task Get_Alert_By_Tenant_idAsync()
    {
        // Arrange
        var tbl = new List<AlertTbl>
        {
            new AlertTbl() // Fill with test data if needed
        };
        var response = tbl; // expected response (should match controller's logic)

        _alertServiceMock
            .Setup(x => x.GetAlertsForTenantAsync(It.IsAny<string>()))
            .ReturnsAsync(tbl);

        _alertController = new AlertController(_alertServiceMock.Object, _mediatorMock.Object);
        // Act
        var result = await _alertController.GetAlertByTenantidAsync("");
        var values = result as ObjectResult;

        // Assert
        Assert.IsNotNull(values);
        Assert.AreEqual(StatusCodes.Status200OK, values?.StatusCode);
        Assert.AreEqual(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(values?.Value));
    }
}
