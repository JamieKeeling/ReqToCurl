using System;

namespace ReqToCurl.Logger
{
    public interface ISimpleLogger<Category> where Category : class
    {
        void Information(string message);

        void Warning(string message);
        void Warning(string message, Exception exception);

        void Error(string message, Exception exception);
    }
}
