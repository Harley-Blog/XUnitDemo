using System;
using XUnitDemo.Infrastucture.Interface;

namespace XUnitDemo.Infrastucture
{
    public class LoggerService : ILoggerService
    {
        public void LogError(string content, Exception ex)
        {
            //向文件中写入日志
        }
    }
}
