using NUnit.Framework;

namespace XUnitDemo.NUnitTests.Product
{
    [TestFixture]
    public class OrderServiceUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(OrderServiceUnitTest)}-{nameof(SetUp)}");
        }

        [Test]
        public void GetOrderInfo_OrderId_ReturnOrderModel()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(OrderServiceUnitTest)}-{nameof(TearDown)}");
        }
    }
}
