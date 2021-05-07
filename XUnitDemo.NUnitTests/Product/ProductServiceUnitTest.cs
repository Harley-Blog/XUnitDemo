using NUnit.Framework;

namespace XUnitDemo.NUnitTests.Product
{
    [TestFixture]
    public class ProductServiceUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(ProductServiceUnitTest)}-{nameof(SetUp)}");
        }

        [Test]
        public void GetProductInfo_ProductId_ReturnProductModel()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(ProductServiceUnitTest)}-{nameof(TearDown)}");
        }
    }
}
