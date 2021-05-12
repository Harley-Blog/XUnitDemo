using NLog;

namespace XUnitDemo.Infrastucture
{
    public class LoggerHelper
    {
        private readonly ILogger _logger = null;
        private static LoggerHelper _loggerHelper = null;
        static LoggerHelper()
        {
            _loggerHelper = new LoggerHelper();
        }

        private LoggerHelper()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public static LoggerHelper Instance => _loggerHelper;

        public void Trace(string content)
        {
            _logger.Trace(content);
        }

        public void Debug(string content)
        {
            _logger.Debug(content);
        }

        public void Info(string content)
        {
            _logger.Info(content);
        }
        public void Warn(string content)
        {
            _logger.Warn(content);
        }
        public void Error(string content)
        {
            _logger.Error(content);
        }

        public void Fatal(string content)
        {
            _logger.Fatal(content);
        }
    }
}
