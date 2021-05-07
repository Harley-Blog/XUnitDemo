using NUnit.Framework;

namespace XUnitDemo.NUnitTests.User
{
    [TestFixture]
    public class UserServiceUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(UserServiceUnitTest)}-{nameof(SetUp)}");
        }

        [Test]
        public void GetUserInfo_UserId_ReturnUserModel()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(UserServiceUnitTest)}-{nameof(TearDown)}");
        }
    }
}
