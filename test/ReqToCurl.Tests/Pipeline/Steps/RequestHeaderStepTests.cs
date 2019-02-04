using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using ReqToCurl.Steps;
using Xunit;

namespace ReqToCurl.Tests.Pipeline.Steps
{
    public class RequestHeaderStepTests
    {
        [Fact]
        public void RequestHeaderExtractor_WithHeaders_CanExtract()
        {
            var headers = new HeaderDictionary
            {
                { "x-header", "value" },
                { "x-another-header", "anothervalue" }
            };

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Headers).Returns(headers);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var requestHeaderStep = new RequestHeaderStep();

            requestHeaderStep.CanExtract(mockHttpContext.Object).Should().BeTrue();
        }

        [Fact]
        public void RequestHeaderExtractor_WithoutHeaders_CannotExtract()
        {
            var headers = new HeaderDictionary();

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Headers).Returns(headers);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var requestHeaderStep = new RequestHeaderStep();

            requestHeaderStep.CanExtract(mockHttpContext.Object).Should().BeFalse();
        }

        [Fact]
        public void RequestHeaderExtractor_WithHeaders_OutputsHeaderString()
        {
            var headers = new HeaderDictionary
            {
                { "x-header", "value" },
                { "x-another-header", "anothervalue" }
            };

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Headers).Returns(headers);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var requestHeaderStep = new RequestHeaderStep();

            var expectedOutput = "-H \"x-header : value\" -H \"x-another-header : anothervalue\"";
            var actualOutput = requestHeaderStep.Extract(mockHttpContext.Object);

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
