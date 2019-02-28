using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using ReqToCurl.Pipeline;
using ReqToCurl.Steps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReqToCurl.Tests.Pipeline
{
    public class PipelineTests
    {
        [Fact]
        public async Task Pipeline_WithSteps_InvokesInjectedSteps()
        {
            var mockHttpContext = Mock.Of<HttpContext>();

            var firstMockStep = new Mock<IExtractionStep>();
            firstMockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Returns(true);
            firstMockStep.Setup(m => m.ExtractAsync(It.IsAny<HttpContext>())).Returns(Task.FromResult("ExtractedContent"));

            var secondMockStep = new Mock<IExtractionStep>();
            secondMockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Returns(true);
            secondMockStep.Setup(m => m.ExtractAsync(It.IsAny<HttpContext>())).Returns(Task.FromResult("ExtractedContent"));

            List<IExtractionStep> steps = new List<IExtractionStep>
            {
                firstMockStep.Object,
                secondMockStep.Object
            };

            var pipeline = new ExtractionPipeline(steps);

            var extractedContent = await pipeline.ExecuteAsync(mockHttpContext);

            extractedContent.Should().NotBeNullOrWhiteSpace();
            firstMockStep.Verify(m => m.CanExtract(mockHttpContext), Times.Once);
            firstMockStep.Verify(m => m.ExtractAsync(mockHttpContext), Times.Once);
            secondMockStep.Verify(m => m.CanExtract(mockHttpContext), Times.Once);
            secondMockStep.Verify(m => m.ExtractAsync(mockHttpContext), Times.Once);
        }

        [Fact]
        public async Task Pipeline_WithStepThatCannotExtract_IsSkipped()
        {
            var mockHttpContext = Mock.Of<HttpContext>();

            var mockStep = new Mock<IExtractionStep>();
            mockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Returns(false);
            mockStep.Setup(m => m.ExtractAsync(It.IsAny<HttpContext>())).Returns(Task.FromResult(string.Empty));

            List<IExtractionStep> steps = new List<IExtractionStep>
            {
                mockStep.Object
            };

            var pipeline = new ExtractionPipeline(steps);

            var extractedContent = await pipeline.ExecuteAsync(mockHttpContext);

            extractedContent.Should().BeNullOrWhiteSpace();
            mockStep.Verify(m => m.CanExtract(mockHttpContext), Times.Once);
            mockStep.Verify(m => m.ExtractAsync(mockHttpContext), Times.Never);
        }

        [Fact]
        public async Task Pipeline_EncountersError_RaisesException()
        {
            var mockHttpContext = Mock.Of<HttpContext>();

            var mockStep = new Mock<IExtractionStep>();
            mockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Throws(new Exception());

            List<IExtractionStep> steps = new List<IExtractionStep>
            {
                mockStep.Object
            };

            var pipeline = new ExtractionPipeline(steps);

            var exception = await Assert.ThrowsAsync<PipelineExtractionFailureException>(() => pipeline.ExecuteAsync(mockHttpContext));
            exception.Message.Should().BeEquivalentTo("Failed to extract from configured steps");
        }
    }
}