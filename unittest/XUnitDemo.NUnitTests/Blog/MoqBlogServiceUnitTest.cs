using Moq;
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
    [Category("*使用隔离框架的测试*")]
    [TestFixture]
    public class MoqBlogServiceUnitTest
    {
        private List<string> SendEmailArgsList = new List<string>();
        private IFileManager _fileManager;
        private ILoggerService _loggerService;
        private IEmailService _emailService;
        private IBlogService _blogService;

        [SetUp]
        public void SetUp()
        {
            //FileManager stub object
            //Return the fake result
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.GetStringFromTxtAsync(It.Is<string>(s => s == Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "Political.txt")))).Returns(Task.FromResult("0000\r\n1111\r\n2222"));
            fileManager.Setup(f => f.GetStringFromTxtAsync(It.Is<string>(s => s == Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "YellowRelated.txt")))).Returns(Task.FromResult("3333\r\n4444\r\n5555"));
            fileManager.Setup(f => f.IsExistsFileAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            _fileManager = fileManager.Object;
            //LoggerService stub object
            //Throw an exception
            var loggerService = new Mock<ILoggerService>();
            loggerService.Setup(s => s.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Throws(new Exception("Custom Exception"));
            _loggerService = loggerService.Object;
            //EmailService mock object
            var emailService = new Mock<IEmailService>();
            emailService
                .Setup(f => f.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => SendEmailArgsList.Clear())
                .Returns(Task.CompletedTask)
                .Callback<string, string, string, string>((arg1, arg2, arg3, arg4) =>
                {
                    SendEmailArgsList.Add(arg1); 
                    SendEmailArgsList.Add(arg2); 
                    SendEmailArgsList.Add(arg3); 
                    SendEmailArgsList.Add(arg4);
                });
            _emailService = emailService.Object;

            _blogService = new BlogService(_fileManager, _loggerService, _emailService);
        }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContent_ReturnSecurityContentAsync()
        {
            //Arrange
            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            string targetContent = "**** **** **** **** **** **** 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";

            //Act
            var blogService = new BlogService(_fileManager, _loggerService, _emailService);
            var result = await _blogService.GetSecurityBlogAsync(originContent);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(result, targetContent, "GetSecurityBlogAsync 未能正确的将内容替换为合法内容");
                CollectionAssert.AreEqual(SendEmailArgsList, new List<string> { "Harley", "System", "LoggerService抛出异常", "Custom Exception" }, "GetSecurityBlogAsync记录错误日志时出现错误，未能正确发送邮件给开发者");
            });

            await Task.CompletedTask;
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
            var loggerService = new Mock<ILoggerService>();
            loggerService.Setup(s => s.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
            var blogService = new BlogService(_fileManager, loggerService.Object, _emailService);
            await blogService.GetSecurityBlogAsync(originContent);
            loggerService.Verify(s => s.LogError(It.IsAny<string>(), It.IsAny<Exception>()), "LoggerService未能正确记录错误消息");
        }

        [Test]
        public async Task GetSecurityBlogAsync_LoggerServiceThrow_SendEmail()
        {
            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            await _blogService.GetSecurityBlogAsync(originContent);
            CollectionAssert.AreEqual(SendEmailArgsList, new List<string> { "Harley", "System", "LoggerService抛出异常", "Custom Exception" }, "GetSecurityBlogAsync记录错误日志时出现错误，未能正确发送邮件给开发者");
        }

        [TearDown]
        public void TearDown()
        { }
    }
}
