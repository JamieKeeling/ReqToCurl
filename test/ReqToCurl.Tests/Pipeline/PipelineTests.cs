using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using ReqToCurl.Pipeline;
using ReqToCurl.Steps;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReqToCurl.Tests.Pipeline
{
    public class PipelineTests
    {
        [Fact]
        public void Pipeline_WithSteps_InvokesInjectedSteps()
        {
            var mockHttpContext = Mock.Of<HttpContext>();

            var firstMockStep = new Mock<IExtractionStep>();
            firstMockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Returns(true);
            firstMockStep.Setup(m => m.Extract(It.IsAny<HttpContext>())).Returns("ExtractedContent");

            var secondMockStep = new Mock<IExtractionStep>();
            secondMockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Returns(true);
            secondMockStep.Setup(m => m.Extract(It.IsAny<HttpContext>())).Returns("ExtractedContent");

            List<IExtractionStep> steps = new List<IExtractionStep>
            {
                firstMockStep.Object,
                secondMockStep.Object
            };

            var pipeline = new ReqToCurl.Pipeline.Pipeline(steps);

            var extractedContent = pipeline.Execute(mockHttpContext);

            extractedContent.Should().NotBeNullOrWhiteSpace();
            firstMockStep.Verify(m => m.CanExtract(mockHttpContext), Times.Once);
            firstMockStep.Verify(m => m.Extract(mockHttpContext), Times.Once);
            secondMockStep.Verify(m => m.CanExtract(mockHttpContext), Times.Once);
            secondMockStep.Verify(m => m.Extract(mockHttpContext), Times.Once);
        }

        [Fact]
        public void Pipeline_WithStepThatCannotExtract_IsSkipped()
        {
            var mockHttpContext = Mock.Of<HttpContext>();

            var mockStep = new Mock<IExtractionStep>();
            mockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Returns(false);
            mockStep.Setup(m => m.Extract(It.IsAny<HttpContext>())).Returns(string.Empty);

            List<IExtractionStep> steps = new List<IExtractionStep>
            {
                mockStep.Object
            };

            var pipeline = new ReqToCurl.Pipeline.Pipeline(steps);

            var extractedContent = pipeline.Execute(mockHttpContext);

            extractedContent.Should().BeNullOrWhiteSpace();
            mockStep.Verify(m => m.CanExtract(mockHttpContext), Times.Once);
            mockStep.Verify(m => m.Extract(mockHttpContext), Times.Never);
        }

        [Fact]
        public void Pipeline_EncountersError_RaisesException()
        {
            var mockHttpContext = Mock.Of<HttpContext>();

            var mockStep = new Mock<IExtractionStep>();
            mockStep.Setup(m => m.CanExtract(It.IsAny<HttpContext>())).Throws(new Exception());

            List<IExtractionStep> steps = new List<IExtractionStep>
            {
                mockStep.Object
            };

            var pipeline = new ReqToCurl.Pipeline.Pipeline(steps);

            var exception = Assert.Throws<PipelineExtractionFailureException>(() => pipeline.Execute(mockHttpContext));
            exception.Message.Should().BeEquivalentTo("Failed to extract from configured steps");
        }
    }
}