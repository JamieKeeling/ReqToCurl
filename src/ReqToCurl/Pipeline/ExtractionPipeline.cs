using Microsoft.AspNetCore.Http;
using ReqToCurl.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqToCurl.Pipeline
{
    public class ExtractionPipeline : IPipeline
    {
        private readonly IEnumerable<IExtractionStep> _extractionSteps;

        public ExtractionPipeline(IEnumerable<IExtractionStep> extractionSteps)
        {
            _extractionSteps = extractionSteps;
        }
        public async Task<string> ExecuteAsync(HttpContext context)
        {
            var extractedContent = new StringBuilder();

            try
            {
                var extractionTasks = _extractionSteps.Where(step => step.CanExtract(context)).Select(s => s.ExtractAsync(context)).ToArray();

                if (extractionTasks.Any())
                {
                    //We don't care about the execution order so collate the Tasks and await them
                    var awaitedOutput = await Task.WhenAll(extractionTasks).ConfigureAwait(false);

                    foreach (var awaitableTask in awaitedOutput)
                    {
                        extractedContent.Append(awaitableTask);
                    }
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
