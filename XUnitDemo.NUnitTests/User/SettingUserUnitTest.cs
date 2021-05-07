using NUnit.Framework;

namespace XUnitDemo.NUnitTests.User
{

    [SetUpFixture]
    public class SettingUserUnitTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(SettingUserUnitTest)}-{nameof(OneTimeSetUp)}");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(SettingUserUnitTest)}-{nameof(OneTimeTearDown)}");
        }
    }
}
