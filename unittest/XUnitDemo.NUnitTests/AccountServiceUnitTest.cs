using NUnit.Framework;
using XUnitDemo.NUnitTests.AOP;

[assembly: Category("NUnit相关的测试")]
namespace XUnitDemo.Tests
{
    [TestFixture]
    [Category("*账号相关的测试*")]
    public class AccountServiceUnitTest
    {
        public void SetUp()
        { }

        [LogTestAction]
        [Test]
        [Category("*用户名认证的测试*")]
        public void Auth_UserNamePwd_ReturnTrue()
        {
            Assert.Pass();
        }

        [Test]
        [Category("*邮箱认证的测试*")]
        public void Auth_EmailPwd_ReturnTrue()
        {
            Assert.Pass();
        }

        [Test]
        [Category("*手机号认证的测试*")]
        public void Auth_MobilePwd_ReturnTrue()
        {
            Assert.Pass();
        }

        [Test]
        [Category("*手机号认证的测试*")]
        public void Auth_MobileCode_ReturnTrue()
        {
            Assert.Pass();
        }
    }
}