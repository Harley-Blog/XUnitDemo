using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XUnitDemo.Infrastucture.Interface;
using XUnitDemo.IService;
using XUnitDemo.Service;

namespace XUnitDemo.NUnitTests.Blog
{
    [TestFixture]
    [Category("BlogService相关测试")]
    public class BlogServiceUnitTest
    {
        private IBlogService _blogService;
        private MockLoggerService _mockLoggerService;
        private StubLoggerService _stubLoggerService;
        private MockEmailService _mockEmailService;

        [SetUp]
        public void SetUp()
        {
            //_mockLoggerService = new MockLoggerService();
            //_blogService = new BlogService(new StubFileManager(), _mockLoggerService);

            _stubLoggerService = new StubLoggerService();
            _mockEmailService = new MockEmailService();
            _blogService = new BlogService(new StubFileManager(), _stubLoggerService, _mockEmailService);
        }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContent_ReturnSecurityContentAsync()
        {
            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            string targetContent = "**** **** **** **** **** **** 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            var result = await _blogService.GetSecurityBlogAsync(originContent);
            Assert.AreEqual(result, targetContent, $"{nameof(_blogService.GetSecurityBlogAsync)} 未能正确的将内容替换为合法内容");
        }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContentIsEmpty_ReturnEmptyAsync()
        {
            string originContent = "";
            var result = await _blogService.GetSecurityBlogAsync(originContent);
            Assert.AreEqual(result, string.Empty, $"{nameof(_blogService.GetSecurityBlogAsync)} 方法参数为空时，返回值也需要为空");
        }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContent_ReturnAllSensitiveListIsEffectiveAsync()
        {
            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            var result = await _blogService.GetSecurityBlogAsync(originContent);
            Assert.AreEqual(true, await _blogService.IsAllSensitiveListIsEffectiveAsync(), $"{nameof(_blogService.GetSecurityBlogAsync)} 敏感词文本列表与系统提供的数量不符");
        }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContent_ErrorMessageIsSendedAsync()
        {
            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            await _blogService.GetSecurityBlogAsync(originContent);
            Assert.AreEqual(_mockLoggerService.LastErrorMessage, $"【{originContent}】含有敏感字符","LoggerService未能正确记录错误消息");
        }

        [Test]
        public async Task GetSecurityBlogAsync_LoggerServiceThrow_SendEmail()
        {
            string error = "Custom Exception";
            _stubLoggerService.Exception = new Exception(error);

            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            await _blogService.GetSecurityBlogAsync(originContent);
            Assert.Multiple(() =>
            {
                Assert.AreEqual("Harley", _mockEmailService.To);
                Assert.AreEqual("System", _mockEmailService.From);
                Assert.AreEqual("LoggerService抛出异常", _mockEmailService.Subject);
                Assert.AreEqual(error, _mockEmailService.Body);
            });
        }

        [TearDown]
        public void TearDown()
        {
            _blogService = null;
        }
    }

    internal class StubFileManager : IFileManager
    {
        public async Task<string> GetStringFromTxtAsync(string filePath)
        {
            var sensitiveList = new List<string> { "Political.txt", "YellowRelated.txt" };
            if (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "Political.txt").Equals(filePath))
            {
                return await Task.FromResult("0000\r\n1111\r\n2222");
            }

            if (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "YellowRelated.txt").Equals(filePath))
            {
                return await Task.FromResult("3333\r\n4444\r\n5555");
            }
            return null;
        }

        public async Task<bool> IsExistsFileAsync(string filePath)
        {
            return await Task.FromResult(true);
        }
    }

    internal class MockLoggerService : ILoggerService
    {
        public string LastErrorMessage { get; set; }
        public void LogError(string content, Exception ex)
        {
            LastErrorMessage = content;
        }
    }

    internal class StubLoggerService : ILoggerService
    {
        public Exception Exception { get; set; }

        public void LogError(string content, Exception ex)
        {
            if (Exception is not null)
            {
                throw Exception;
            }
        }
    }

    internal class MockEmailService : IEmailService
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public async Task SendEmailAsync(string to, string from, string subject, string body)
        {
            To = to;
            From = from;
            Subject = subject;
            Body = body;
            await Task.CompletedTask;
        }
    }
}
