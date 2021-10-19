using Microsoft.Extensions.Logging;
using System;

namespace ReqToCurl.Logger
{
    public class SimpleLogger<Category> : ISimpleLogger<Category> where Category : class
    {
        private readonly ILogger<Category> _logger;
        public SimpleLogger(ILogger<Category> logger)
        {
            _logger = logger;
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.LogError(exception, message);
        }

        public void Information(string message)
        {
            _logger.LogInformation(message);
        }

        public void Warning(string message)
        {
            _logger.LogWarning(message);
        }

        public void Warning(string message, Exception exception)
        {
            _logger.LogWarning(exception, message);
        }
    }
}
