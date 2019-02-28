using System;

namespace ReqToCurl.Pipeline
{
    public class PipelineExtractionFailureException : Exception
    {
        public PipelineExtractionFailureException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
