using NUnit.Framework;

namespace XUnitDemo.NUnitTests.Product
{

    [SetUpFixture]
    public class SettingProductUnitTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(SettingProductUnitTest)}-{nameof(OneTimeSetUp)}");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        { 
            System.Diagnostics.Debug.WriteLine($"{nameof(SettingProductUnitTest)}-{nameof(OneTimeTearDown)}");
        }
    }
}
