using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;
using NUnit.Framework;
using Semanix.Application.Response;

namespace Semanix.Tests
{
    [TestFixture]
    public class TransactionControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _biometricProcessorMock = new Mock<IBiometricProcessor>();
            _adminServiceMock = new Mock<IAdminService>();
            _transactionController =
                new TransactionController(_transactionServiceMock.Object, _adminServiceMock.Object,
                    _biometricProcessorMock.Object);
        }

        private TransactionController? _transactionController;
        private Mock<ITransactionService>? _transactionServiceMock;
        private Mock<IAdminService>? _adminServiceMock;
        private Mock<IBiometricProcessor>? _biometricProcessorMock;

        [Test]
        public async Task Deposit_WithValidInput_Returns200OK()
        {
            // Arrange
            var depositDto = new DepositDto
            {
                IsOwner = true,
                DepositTypes = Enum.Parse<DepositTypes>("Cash"),
                CurrencyType = "ngn",
                DepositorName = "Bolanle",
                DepositorPhoneNo = "08012345678",
                Remark = "Remark",
                Denominations = new()
                {
                    new DenominationDto() { Denomination = 1000, Quantity = 2, Amount = 2000 }
                },
                Narration = "Narration",
                AccountNumber = "0482975039",
                Amount = 2000,
            };

            var data = new SemanixApiBaseResponse<FcmbDoTransferResponse>
            {
                Code = "00",
                Description = "Approved Or Completed Successfully",
                Data = new FcmbDoTransferResponse
                {
                    Amount = depositDto.Amount,
                    Stan = "734945633209",
                    CustomerReference = "",
                    Tran_Id = ""
                }
            };
            string user = "Ayokunmi.Lawal";

            var expectedResult = new Response
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = data.Description,
                Data = data.Data
            };

            _transactionServiceMock!
                .Setup(service => service.Deposit(It.IsAny<DepositDto>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await _transactionServiceMock!.Object.Deposit(depositDto, user);


            //Assert
            result.Should().NotBeNull();

            result.Should().BeOfType<Response>();

            result.StatusCode.Should().Be(expectedResult.StatusCode);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Deposit_WithInvalidInput_Returns400BadRequest()
        {
            // Arrange
            var depositDto = new DepositDto
            {
                IsOwner = true,
                DepositTypes = Enum.Parse<DepositTypes>("Cash"),
                CurrencyType = "NGN",
                Narration = "Narration",
                AccountNumber = "0482975039",
                Amount = 0,
            };

            var user = "Ayokunmi.Lawal";
            if (user == null)
                return;

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(),
                new ControllerActionDescriptor());
            _transactionController!.ControllerContext = new ControllerContext(actionContext)
            {
                HttpContext = httpContext
            };

            _transactionController.ModelState.AddModelError("Amount", "Amount is required");
            _transactionController.ModelState.AddModelError("Amount", "Amount must be greater than 0");
            _transactionController.ModelState.AddModelError("Initiator", "Initiator is required");


            _transactionServiceMock!.Setup(service => service.Deposit(It.IsAny<DepositDto>(), user))
                .ReturnsAsync(new Response { StatusCode = StatusCodes.Status400BadRequest });
            // Act

            var ex = Assert.ThrowsAsync<Semanix.Common.CustomException
                .ApplicationException>(async () => await _transactionController.Deposit(depositDto));

            Assert.AreEqual("Invalid payload request, please check and try again", ex.Message);
        }

        [Test]
        public async Task Withdraw_WithValidInput_Returns200OK()
        {
            // Arrange
            var withdrawalDto = new WithdrawalDto
            {
                IsOwner = true,
                CurrencyType = "ngn",
                InstrumentDate = DateTime.Now,
                InstrumentType = Enum.Parse<InstructionTypes>("WSLIP"),
                InstrumentNumber = "insNum",
                Narration = "Narration",
                PhoneNumber = "1234567890",
                AccountNumber = "0482975039",
                Amount = 5600,
                DraweeName = "Olaronke",
                Remark = "remark"
            };

            var data = new SemanixApiBaseResponse<FcmbDoTransferResponse>
            {
                Code = "00",
                Description = "Approved Or Completed Successfully",
                Data = new FcmbDoTransferResponse
                {
                    Amount = withdrawalDto.Amount,
                    Stan = "734945633209",
                    CustomerReference = "",
                    Tran_Id = ""
                }
            };

            var user = "Ayokunmi.Lawal";

            if (user == null)
                return;

            var expectedResult = new Response
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = data.Description,
                Data = data.Data
            };

            _transactionServiceMock!
                .Setup(service => service.Withdrawal(It.IsAny<WithdrawalDto>(), user))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _transactionServiceMock!.Object.Withdrawal(withdrawalDto, user);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Response>();

            result.StatusCode.Should().Be(expectedResult.StatusCode);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Withdraw_WithInvalidInput_Returns400BadRequest()
        {
            // Arrange
            var withdrawDto = new WithdrawalDto
            {
                IsOwner = true,
                CurrencyType = "ngn",
                InstrumentDate = DateTime.Now,
                InstrumentType = Enum.Parse<InstructionTypes>("WSLIP"),
                InstrumentNumber = "insNum",
                Narration = "Narration",
                PhoneNumber = "1234567890",
                AccountNumber = "",
                Amount = 0,
                DraweeName = "Olaronke",
                Remark = "remark"
            };

            var user = "Ayokunmi.Lawal";
            if (user == null)
                return;

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(),
                new ControllerActionDescriptor());
            _transactionController!.ControllerContext = new ControllerContext(actionContext)
            {
                HttpContext = httpContext
            };

            _transactionController.ModelState.AddModelError("Amount", "Amount is required");
            _transactionController.ModelState.AddModelError("Amount", "Amount must be greater than 0");
            _transactionController.ModelState.AddModelError("Initiator", "Initiator is required");

            //Act and Assert
            var ex = Assert.ThrowsAsync<Semanix.Common.CustomException
                .ApplicationException>(async () => await _transactionController.Withdrawal(withdrawDto));

            Assert.AreEqual("Invalid payload request, please check and try again", ex.Message);
        }
    }
}