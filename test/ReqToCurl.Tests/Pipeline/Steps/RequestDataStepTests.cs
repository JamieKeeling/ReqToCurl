using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using ReqToCurl.Steps;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReqToCurl.Tests.Pipeline.Steps
{
    public class RequestDataStepTests
    {
        [Fact]
        public void RequestDataStep_WithRequestData_CanExtract()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("bodycontent"));

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Body).Returns(memoryStream);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var requestDataStep = new RequestDataStep();

            requestDataStep.CanExtract(mockHttpContext.Object).Should().BeTrue();
        }

        [Fact]
        public void RequestDataStep_WithoutRequestData_CannotExtract()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Body).Returns(memoryStream);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var requestDataStep = new RequestDataStep();

            requestDataStep.CanExtract(mockHttpContext.Object).Should().BeFalse();
        }

        [Fact]
        public async Task RequestDataStep_WithRequestData_OutputsDataString()
        {
            var jsonPayload = JsonConvert.SerializeObject(new { Name = "Jamie" });
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonPayload));

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(p => p.Body).Returns(memoryStream);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(mockHttpRequest.Object);

            var requestDataStep = new RequestDataStep();

            var extractedData = await requestDataStep.ExtractAsync(mockHttpContext.Object);

            extractedData.Should().NotBeNullOrWhiteSpace();
        }
    }
}
