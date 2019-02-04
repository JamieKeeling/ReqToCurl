using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using Xunit;

namespace ReqToCurl.Tests.String_Output
{
    public class CurlExtractorTests
    {
        [Fact]
        public void Extractor_WithRequest_OutputsBaseCommand()
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

            var extractor = new CurlExtractor(null);

            var expectedOutput = $"curl {httpScheme}://{host}{path.Value}{queryString}" + Environment.NewLine;
            var actualOutput = extractor.ExtractRequest(mockHttpContext.Object);

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
