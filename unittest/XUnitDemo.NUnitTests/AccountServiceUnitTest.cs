using NUnit.Framework;
using XUnitDemo.NUnitTests.AOP;

[assembly: Category("NUnit��صĲ���")]
namespace XUnitDemo.Tests
{
    [TestFixture]
    [Category("*�˺���صĲ���*")]
    public class AccountServiceUnitTest
    {
        public void SetUp()
        { }

        [LogTestAction]
        [Test]
        [Category("*�û�����֤�Ĳ���*")]
        public void Auth_UserNamePwd_ReturnTrue()
        {
            Assert.Pass();
        }

        [Test]
        [Category("*������֤�Ĳ���*")]
        public void Auth_EmailPwd_ReturnTrue()
        {
            Assert.Pass();
        }

        [Test]
        [Category("*�ֻ�����֤�Ĳ���*")]
        public void Auth_MobilePwd_ReturnTrue()
        {
            Assert.Pass();
        }

        [Test]
        [Category("*�ֻ�����֤�Ĳ���*")]
        public void Auth_MobileCode_ReturnTrue()
        {
            Assert.Pass();
        }
    }
}