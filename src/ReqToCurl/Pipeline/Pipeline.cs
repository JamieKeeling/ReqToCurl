using Microsoft.AspNetCore.Http;
using ReqToCurl.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReqToCurl.Pipeline
{
    public class Pipeline : IPipeline
    {
        private readonly IEnumerable<IExtractionStep> _extractionSteps;

        public Pipeline(IEnumerable<IExtractionStep> extractionSteps)
        {
            _extractionSteps = extractionSteps;
        }
        public string Execute(HttpContext context)
        {
            var extractedContent = new StringBuilder();

            try
            {
                foreach (IExtractionStep extractionStep in _extractionSteps.Where(step => step.CanExtract(context)))
                {
                    extractedContent.Append(extractionStep.Extract(context));
                }
            }
            catch (Exception ex)
            {
                throw new PipelineExtractionFailureException("Failed to extract from configured steps", ex);
            }          

            return extractedContent.Length > 0 ? extractedContent.ToString() : null;
        }
    }
}
