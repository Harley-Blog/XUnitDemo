using System;

namespace XUnitDemo.Infrastucture.Interface
{
    public interface ILoggerService
    {
        public void LogError(string content, Exception ex);
    }
}
