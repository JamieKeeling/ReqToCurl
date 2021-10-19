using Microsoft.AspNetCore.Http;
using Moq;
using ReqToCurl.Logger;
using System.Threading.Tasks;
using Xunit;

namespace ReqToCurl.Tests
{
    public class InvocationTests
    {
        [Fact]
        public async void Invoked_WhenResponseHasStarted_IsTrue()
        {
            var mockLogger = new Mock<ISimpleLogger<RequestToCurlMiddleware>>();

            var middlewareInstance = new RequestToCurlMiddleware((innerHttpContext) => Task.FromResult(0), mockLogger.Object, Mock.Of<ICurlExtractor>());

            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpResponse.SetupGet(p => p.HasStarted).Returns(true);
            mockHttpResponse.SetupGet(p => p.StatusCode).Returns(200);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Response).Returns(mockHttpResponse.Object);

            await middlewareInstance.InvokeAsync(mockHttpContext.Object);

            mockLogger.Verify(x => x.Information("Response has started with a HTTP 200 value"), Times.Once);
        }

        [Fact]
        public async void NotInvoked_WhenResponseHasStarted_IsFalse()
        {
            var mockLogger = new Mock<ISimpleLogger<RequestToCurlMiddleware>>();

            var middlewareInstance = new RequestToCurlMiddleware((innerHttpContext) => Task.FromResult(0), mockLogger.Object, Mock.Of<ICurlExtractor>());

            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpResponse.SetupGet(p => p.HasStarted).Returns(false);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Response).Returns(mockHttpResponse.Object);

            await middlewareInstance.InvokeAsync(mockHttpContext.Object);

            mockLogger.Verify(x => x.Information(It.IsAny<string>()), Times.Never);
        }
    }
}