using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace XUnitDemo.Tests
{
    [TestFixture]
    public class AccountCountrollerUnitTest
    {
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