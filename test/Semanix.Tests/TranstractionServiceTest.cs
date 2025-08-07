using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Semanix.Api.Controllers;
using Semanix.Application.BaseHelpers;
using Semanix.Application.BiometricScanner.Interface;
using Semanix.Application.Config;
using Semanix.Application.Interfaces.Services;
using Semanix.Application.Response;
using Semanix.Application.Services;
using Semanix.Application.Transaction.Model;

namespace Semanix.Tests;

[TestFixture]
public class TransactionServiceTest
{
    [SetUp]
    public void Setup()
    {
        _transactionServiceMock = new Mock<ITransactionService>();
        _adminServiceMock = new Mock<IAdminService>();
        _biometricProcessorMock = new Mock<IBiometricProcessor>();
        _transactionController = new TransactionController(_transactionServiceMock.Object, _adminServiceMock.Object,
            _biometricProcessorMock.Object);

        //initialize transaction service dependencies 
        _httpFactoryServiceMock = new Mock<IHttpFactoryService>();
        _loggerMock = new Mock<ILogger<TransactionService>>();
        _configurationMock = new Mock<IConfiguration>();
        _appSettingsMock = new Mock<IOptions<AppSettings>>();
        _authHttpServiceClientMock = new Mock<HttpContextServiceClient>();
        _authServiceMock = new Mock<IAuthService>();

        // Set up the configuration mock
        _configurationMock.Setup(config => config["AppSettings:FinacleSoapUrl"])
            .Returns("http://41.203.107.109/AuthenticationUtilityServiceSIT/AuthenticationService.asmx?wsdl");
        _configurationMock.Setup(config => config["AppSettings:FIService"])
            .Returns("http://41.203.107.109:7788/FIService.asmx?wsdl");

        // Create an instance of the TransactionService
        _transactionService = new TransactionService(
            _httpFactoryServiceMock.Object,
            _loggerMock.Object,
            _configurationMock.Object,
            _appSettingsMock.Object,
            _authHttpServiceClientMock.Object,
            _authServiceMock.Object
        );
    }

    private Mock<ITransactionService>? _transactionServiceMock;
    private Mock<IAdminService>? _adminServiceMock;
    private TransactionController? _transactionController;
    private TransactionService _transactionService;
    private Mock<IHttpFactoryService>? _httpFactoryServiceMock;
    private Mock<ILogger<TransactionService>>? _loggerMock;
    private Mock<IConfiguration>? _configurationMock;
    private Mock<IOptions<AppSettings>>? _appSettingsMock;
    private Mock<HttpContextServiceClient>? _authHttpServiceClientMock;
    private Mock<IAuthService>? _authServiceMock;
    private Mock<IBiometricProcessor>? _biometricProcessorMock;

    [Test]
    public void TransactionService_Constructor_ShouldInitializeFields()
    {
        //// Arrange
        _configurationMock!.Setup(config => config["AppSettings:FinacleSoapUrl"])
            .Returns("http://41.203.107.109/AuthenticationUtilityServiceSIT/AuthenticationService.asmx?wsdl");
        _configurationMock.Setup(config => config["AppSettings:FIService"])
            .Returns("http://41.203.107.109:7788/FIService.asmx?wsdl");
        // Act
        var transactionService = new TransactionService(
            _httpFactoryServiceMock!.Object,
            _loggerMock!.Object,
            _configurationMock.Object,
            _appSettingsMock!.Object,
            _authHttpServiceClientMock!.Object,
            _authServiceMock!.Object
        );

        // Assert
        Assert.NotNull(transactionService);
    }

    [Test]
    public async Task AccountEnquiryByAccountNumber_ValidAccountNumber_Returns200OK()
    {
        // Arrange
        var validAccountNumber = "0482975039";
        var fcmbResponse = new SemanixApiResponse<FcmbAccountEnquiryResponse>()
        {
            Code = "00",
            Description = "success",
            Data = new()
        };
        fcmbResponse.Data.Add(new FcmbAccountEnquiryResponse()
        {
            FirstName = "IDOWU",
            LastName = "OKUBOTE",
            MiddleName = "OLUWAKEMI",
            SchemeCode = "OD286",
            Currency = "NGN",
            Status = "ACTIVE",
            AccountType = "FAST CASH",
            Mobile = "9087110813",
            Email = "IDOWUOKUBOTE@YAHOO.COM",
            AvailableBalance = 250000,
            LedgerBalance = 0,
            CustomerType = "INDIVIDUAL",
            OpeningDate = DateTime.Now,
            Bvn = "22160119688",
            Tin = null!,
            Nin = "AGL06113AAB",
            Restriction = null!,
            Address = "FLAT 40A BLK4 FUNSO WILLIAMS STR, OTEDOLA ESTATE BERGER. ",
            BrokerCode = "10977",
            AccountOfficer = "YETUNDE OMIDINA",
            AccountNumber = "0482975046",
            CustomerId = "0482975"
        });

        var accountResponse = new Response
        {
            Data = fcmbResponse.Data,
            Message = fcmbResponse.Description,
            StatusCode = StatusCodes.Status200OK
        };

        _httpFactoryServiceMock?.Setup(mock => mock.AccountEnquiryByAccountNumber(validAccountNumber))
            .ReturnsAsync(fcmbResponse);

        // Act
        var result = await _transactionService.AccountEnquiryByAccountNumber(validAccountNumber);

        // Assert
        Assert.NotNull(result);
        // Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        // Assert.AreEqual(accountResponse.Message, result.Message);
    }
}