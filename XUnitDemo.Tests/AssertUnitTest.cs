using NUnit.Framework;
using System;
using System.Reflection;

namespace XUnitDemo.Tests
{
    [TestFixture]
    public class AssertUnitTest
    {
        public AssertUnitTest()
        {
            System.Diagnostics.Debug.WriteLine($"This is {nameof(AssertUnitTest)}");
        }

        [Test]
        public void Login_UserNamePwd_ReturnTrue()
        {
            Assert.Inconclusive($"This is {nameof(Login_UserNamePwd_ReturnTrue)}");
        }

        [Test]
        public void IsAssignableFrom_TypeObject_ReturnVoid()
        {
            Assert.IsAssignableFrom(typeof(string), nameof(IsAssignableFrom_TypeObject_ReturnVoid));
            Assert.IsAssignableFrom(typeof(int), 123);
        }

        [Test]
        public void IsEmpty_String_ReturnVoid()
        {
            string str1 = nameof(IsAssignableFrom_TypeObject_ReturnVoid);
            string str2 = null;
            string str3 = "";
            string str4 = string.Empty;
            Assert.IsEmpty(str4);
            Assert.IsEmpty(str3);
            Assert.IsEmpty(str2);
            Assert.IsEmpty(str1);
        }

        [Test]
        public void IsFalse_Condition_ReturnVoid()
        {
            var con1 = 1 > 0;
            var con2 = string.IsNullOrEmpty(nameof(IsFalse_Condition_ReturnVoid));
            Assert.IsFalse(con2);
            Assert.IsFalse(con1);
        }

        [Test]
        public void IsInstanceOf_TypeObject_ReturnVoid()
        {
            var str1 = MethodBase.GetCurrentMethod().Name;
            var date = DateTime.Now;
            var num = 5;
            Assert.IsInstanceOf(typeof(string), str1);
            Assert.IsInstanceOf(typeof(DateTime), date);
            Assert.IsInstanceOf(typeof(long), num);
        }

        [Test]
        public void IsNan_TypeObject_ReturnVoid()
        {
            dynamic obj = new { First = 123, Second = default(double?), Third = -1 };
            Assert.IsNaN(Math.Sqrt(obj.Third));
            Assert.IsNaN(obj.Second);
            Assert.IsNaN(obj.First);
        }

        [Test]
        public void IsNotAssignableFrom_TypeObject_ReturnVoid()
        {
            var str1 = MethodBase.GetCurrentMethod().Name;
            var date = DateTime.Now;
            var num = 5;
            Assert.IsNotAssignableFrom(typeof(long), num);
            Assert.IsNotAssignableFrom(typeof(string), str1);
            Assert.IsNotAssignableFrom(typeof(DateTime), date);
        }

        [Test]
        public void IsNotEmpty_TypeObject_ReturnVoid()
        {
            string str1 = nameof(IsNotEmpty_TypeObject_ReturnVoid);
            string str2 = null;
            string str3 = "";
            string str4 = string.Empty;
            Assert.IsNotEmpty(str1);
            Assert.IsNotEmpty(str2);
            Assert.IsNotEmpty(str3);
            Assert.IsNotEmpty(str4);
        }

        [Test]
        public void IsNotInstanceOf_TypeObject_ReturnVoid()
        {
            var str1 = MethodBase.GetCurrentMethod().Name;
            var date = DateTime.Now;
            var num = 5;
            Assert.IsNotInstanceOf(typeof(long), num);
            Assert.IsNotInstanceOf(typeof(string), str1);
            Assert.IsNotInstanceOf(typeof(DateTime), date);
        }

        [Test]
        public void Less_Arg1Arg2_ReturnVoid()
        {
            Assert.Less(1, 2);
            Assert.Less(1, 1);
            Assert.Less(2,1);
        }

        [SetUp]
        public void Init()
        {
            System.Diagnostics.Debug.WriteLine($"This is {nameof(Init)}");
        }
        [SetUp]
        public void Init2()
        {
            System.Diagnostics.Debug.WriteLine($"This is {nameof(Init2)}");
        }

        [TearDown]
        public void Release()
        {
            System.Diagnostics.Debug.WriteLine($"This is {nameof(Release)}");
        }

        [TearDown]
        public void Release2()
        {
            System.Diagnostics.Debug.WriteLine($"This is {nameof(Release2)}");
        }

        ~AssertUnitTest()
        {
            System.Diagnostics.Debug.WriteLine($"This is ~{nameof(AssertUnitTest)}");
        }
    }
}
