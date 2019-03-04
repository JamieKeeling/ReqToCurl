using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using ReqToCurl.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ReqToCurl.Steps;
using Xunit;

namespace ReqToCurl.Tests.Output
{
    public class CurlExtractorTests
    {
        [Fact]
        public async Task Extractor_WithRequest_OutputsBaseCommand()
        {
            const string httpScheme = "https";
            var host = new HostString("tempurl.com:12345");
            var queryString = new QueryString("?x=1&y=2&z=3");
            var path = new PathString("/path1/path2/path3");

            var expectedOutput = $"curl {httpScheme}://{host}{path.Value}{queryString}" + Environment.NewLine;

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Scheme).Returns(httpScheme);
            mockHttpRequest.SetupGet(p => p.Host).Returns(host);
            mockHttpRequest.SetupGet(p => p.QueryString).Returns(queryString);
            mockHttpRequest.SetupGet(p => p.Path).Returns(path);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var extractor = new CurlExtractor(Mock.Of<IPipeline>());

            var actualOutput = await extractor.ExtractRequestAsync(mockHttpContext.Object);

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public async Task Extractor_WithRequest_InvokesExtractionPipeline()
        {
            const string httpScheme = "https";
            var host = new HostString("tempurl.com:12345");
            var queryString = new QueryString("?x=1&y=2&z=3");
            var path = new PathString("/path1/path2/path3");

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Scheme).Returns(httpScheme);
            mockHttpRequest.SetupGet(p => p.Host).Returns(host);
            mockHttpRequest.SetupGet(p => p.QueryString).Returns(queryString);
            mockHttpRequest.SetupGet(p => p.Path).Returns(path);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var mockExtractionPipeline = new Mock<IPipeline>();
            mockExtractionPipeline.Setup(m => m.ExecuteAsync(It.IsAny<HttpContext>())).ReturnsAsync(string.Empty);

            var extractor = new CurlExtractor(mockExtractionPipeline.Object);

            await extractor.ExtractRequestAsync(mockHttpContext.Object);

            mockExtractionPipeline.Verify(m => m.ExecuteAsync(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task Extractor_WithRequest_IncludesExtractedPipelineData()
        {
            const string httpScheme = "https";
            var host = new HostString("tempurl.com:12345");
            var queryString = new QueryString("?x=1&y=2&z=3");
            var path = new PathString("/path1/path2/path3");
            var pipelineResponse = "pipelinecontent";

            var expectedOutput = $"curl {httpScheme}://{host}{path.Value}{queryString}{Environment.NewLine}{pipelineResponse}{Environment.NewLine}";

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Scheme).Returns(httpScheme);
            mockHttpRequest.SetupGet(p => p.Host).Returns(host);
            mockHttpRequest.SetupGet(p => p.QueryString).Returns(queryString);
            mockHttpRequest.SetupGet(p => p.Path).Returns(path);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var mockExtractionPipeline = new Mock<IPipeline>();
            mockExtractionPipeline.Setup(m => m.ExecuteAsync(It.IsAny<HttpContext>())).ReturnsAsync(pipelineResponse);

            var extractor = new CurlExtractor(mockExtractionPipeline.Object);

            var actualOutput = await extractor.ExtractRequestAsync(mockHttpContext.Object);

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
