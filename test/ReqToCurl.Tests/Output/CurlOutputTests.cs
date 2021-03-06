﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReqToCurl.Tests.Output
{
    public class CurlOutputTests
    {
        [Fact]
        public async void Invoked_WithMinimumRequest_OutputsCurlString()
        {
            var curlString = $"curl https://tempurl.com:12345/path1/path2/path3?x=1&y=2&z=3" + Environment.NewLine;
            var mockCurlExtractor = new Mock<ICurlExtractor>();
            mockCurlExtractor.Setup(m => m.ExtractRequestAsync(It.IsAny<HttpContext>())).ReturnsAsync(curlString);

            var mockLogger = new Mock<ILogger<RequestToCurlMiddleware>>();

            var middlewareInstance = new RequestToCurlMiddleware((innerHttpContext) => Task.FromResult(0), mockLogger.Object, mockCurlExtractor.Object);

            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpResponse.SetupGet(p => p.HasStarted).Returns(true);
            mockHttpResponse.SetupGet(p => p.StatusCode).Returns(200);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Response).Returns(mockHttpResponse.Object);

            await middlewareInstance.InvokeAsync(mockHttpContext.Object);

            mockLogger.Verify(x => x.Log(LogLevel.Information,
                                            It.IsAny<EventId>(),
                                            new FormattedLogValues(curlString),
                                            null,
                                            It.IsAny<Func<object, Exception, string>>()),
                                            Times.Once);
        }
    }
}
