using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XUnitDemo.Infrastucture.Interface;
using XUnitDemo.Service;

namespace XUnitDemo.NUnitTests.Blog
{
    [Category("*使用隔离框架的测试*")]
    [TestFixture]
    public class DynamicMockBlogServiceUnitTest
    {
        [SetUp]
        public void SetUp()
        { }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContent_ReturnSecurityContentAsync()
        {
            //Arrange
            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            string targetContent = "**** **** **** **** **** **** 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            //FileManager stub object
            //Return the fake result
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.GetStringFromTxtAsync(It.Is<string>(s => s == Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "Political.txt")))).Returns(Task.FromResult("0000\r\n1111\r\n2222"));
            fileManager.Setup(f => f.GetStringFromTxtAsync(It.Is<string>(s => s == Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "YellowRelated.txt")))).Returns(Task.FromResult("3333\r\n4444\r\n5555"));
            fileManager.Setup(f => f.IsExistsFileAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            //LoggerService stub object
            //Throw an exception
            var loggerService = new Mock<ILoggerService>();
            loggerService.Setup(s => s.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Throws(new Exception("Custom Exception"));
            //EmailService mock object
            var emailService = new Mock<IEmailService>();
            var list = new List<string>();
            emailService
                .Setup(f => f.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback<string, string, string, string>((arg1, arg2, arg3, arg4) =>
                {
                    list.Add(arg1); list.Add(arg2); list.Add(arg3); list.Add(arg4);
                });


            //Act
            var blogService = new BlogService(fileManager.Object, loggerService.Object, emailService.Object);
            var result = await blogService.GetSecurityBlogAsync(originContent);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(result, targetContent, "GetSecurityBlogAsync 未能正确的将内容替换为合法内容");
                CollectionAssert.AreEqual(list, new List<string> { "Harley", "System", "LoggerService抛出异常", "Custom Exception" }, "GetSecurityBlogAsync记录错误日志时出现错误，未能正确发送邮件给开发者");
            });

            await Task.CompletedTask;
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
