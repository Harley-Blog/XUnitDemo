using NUnit.Framework;

namespace XUnitDemo.NUnitTests.User
{
    [TestFixture]
    public class UserMenuServiceUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(UserMenuServiceUnitTest)}-{nameof(SetUp)}");
        }

        [Test]
        public void GetUserMenuList_UserId_ReturnMenuList()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(UserMenuServiceUnitTest)}-{nameof(TearDown)}");
        }
    }
}
