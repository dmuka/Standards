using System.Net;
using FluentValidation;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Infrastructure.Exceptions;

namespace Tests.Infrastructure.Exceptions;

public class ExceptionHandlingMiddlewareTests
{
    private Mock<RequestDelegate> _nextMock;
    private Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
    private ExceptionHandlingMiddleware _middleware;

    [SetUp]
    public void Setup()
    {
        _nextMock = new Mock<RequestDelegate>();
        _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _middleware = new ExceptionHandlingMiddleware(_nextMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Invoke_WhenNoException_ShouldCallNextDelegate()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        // Act
        await _middleware.Invoke(httpContext);

        // Assert
        _nextMock.Verify(next => next(httpContext), Times.Once);
    }

    [Test]
    public async Task Invoke_WhenValidationException_ShouldReturnBadRequest()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _nextMock.Setup(next => next(It.IsAny<HttpContext>()))
            .ThrowsAsync(new ValidationException("Validation error"));

        // Act
        await _middleware.Invoke(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Invoke_WhenStandardsException_ShouldReturnCustomStatusCode()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var standardsException = new StandardsException(StatusCodeByError.BadRequest, "WebApi error", "WebApi error", true);
        _nextMock.Setup(next => next(It.IsAny<HttpContext>()))
            .ThrowsAsync(standardsException);

        // Act
        await _middleware.Invoke(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Invoke_WhenGenericException_ShouldReturnInternalServerError()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _nextMock.Setup(next => next(It.IsAny<HttpContext>()))
            .ThrowsAsync(new Exception("Generic error"));

        // Act
        await _middleware.Invoke(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
    }
}