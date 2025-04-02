// using System.Net;
// using Application.Builders;
// using MediatR;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Presentation.Controllers;
// using Shared.Commands;
// using Shared.Enums;
// using Shared.Responses;
//
// namespace Tests.Presentation.Controllers;
//
// public class ControllerTests
// {
//     private readonly AuthController _controller;
//     private readonly Mock<IHttpResponseBuilder> _mockResponseBuilder;
//     private readonly Mock<ISender> _mockSender;
//
//     public ControllerTests()
//     {
//         // Mock dependencies
//         _mockResponseBuilder = new Mock<IHttpResponseBuilder>();
//         _mockSender = new Mock<ISender>();
//
//         // Initialize controller with mocks
//         _controller = new AuthController
//         (
//             _mockSender.Object
//         );
//     }
//
//     [Fact]
//     public async Task ProcessUserRegistrationAsync_ShouldReturnBadRequest_WhenModelStateIsInvalid()
//     {
//         // Arrange
//         var command = new CreateUserCommand { Email = "", Password = "123" }; // Invalid
//         _controller.ModelState.AddModelError("Email", "Email is required.");
//
//         var expectedErrors = new List<string> { "Email is required." };
//
//         _mockResponseBuilder
//             .Setup(r => r.CreateResponse((int)HttpStatusCode.BadRequest, expectedErrors, null))
//             .Returns(new ControllerResponse<List<string>>
//             {
//                 IsSuccess = false,
//                 Message = HttpStatusDescriptions.GetDescription((int)HttpStatusCode.BadRequest),
//                 Data = expectedErrors,
//                 StatusCode = (int)HttpStatusCode.BadRequest
//             });
//
//         // Act
//         var result = await _controller.ProcessUserRegistrationAsync(command);
//
//         // Assert
//         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//         Assert.Equal(400, badRequestResult.StatusCode);
//     }
//
//     [Fact]
//     public async Task ProcessUserRegistrationAsync_ShouldReturnBadRequest_WhenUserCreationFails()
//     {
//         // Arrange
//         var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure123!" };
//         var failedResult = IdentityResult.Failed(new IdentityError { Description = "User creation failed" });
//
//         // _mockUserCommandHandler
//         //     .Setup(u => u.Handle(command, CancellationToken.None))
//         //     .ReturnsAsync(failedResult);
//
//         _mockResponseBuilder
//             .Setup(r => r.CreateResponse((int)HttpStatusCode.BadRequest, failedResult.Errors, null))
//             .Returns(new ControllerResponse<IEnumerable<IdentityError>>
//             {
//                 Message = HttpStatusDescriptions.GetDescription((int)HttpStatusCode.BadRequest),
//                 StatusCode = (int)HttpStatusCode.BadRequest,
//                 Data = failedResult.Errors,
//                 IsSuccess = false
//             });
//
//         // Act
//         var result = await _controller.ProcessUserRegistrationAsync(command);
//
//         // Assert
//         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//         Assert.Equal(400, badRequestResult.StatusCode);
//     }
//     [Fact]
//     public async Task ProcessUserRegistrationAsync_ShouldReturn_Created_WhenUserCreatedSuccessfully()
//     {
//         // Arrange
//         var command = new CreateUserCommand{Email = "test@example.com",Password = "P@ssw0rd!"};
//         var successResult = IdentityResult.Success; // Simulating a successful identity result
//
//         _mockSender
//             .Setup(sender => sender.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(successResult);
//
//         // Act
//         var result = await _controller.ProcessUserRegistrationAsync(command);
//
//         // Assert
//         var objectResult = Assert.IsType<OkObjectResult>(result);
//         Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
//     }
//
//     [Fact]
//     public async Task ProcessUserRegistrationAsync_ShouldReturn_BadRequest_WhenUserCreationFails()
//     {
//         // Arrange
//         var command = new CreateUserCommand{Email = "test@example.com",Password = "P@ssw0rd!"};
//         var failureResult = IdentityResult.Failed(new IdentityError { Description = "Error creating user" });
//
//         _mockSender
//             .Setup(sender => sender.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(failureResult);
//
//         // Act
//         var result = await _controller.ProcessUserRegistrationAsync(command);
//
//         // Assert
//         var objectResult = Assert.IsType<BadRequestObjectResult>(result);
//         Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
//     }
// }
