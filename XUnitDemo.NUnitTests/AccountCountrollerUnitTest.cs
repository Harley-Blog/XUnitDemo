using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace XUnitDemo.Tests
{
    [TestFixture]
    public class AccountCountrollerUnitTest
    {
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