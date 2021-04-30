using NUnit.Framework;

namespace XUnitDemo.Tests
{
    [TestFixture()]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void TestIgnore()
        {
            Assert.Inconclusive("这个方法不进行测试");
        }

        [Test]
        public void AccountCountroller_UserNamePwd_ReturnTrue()
        { 
            
        }
    }
}