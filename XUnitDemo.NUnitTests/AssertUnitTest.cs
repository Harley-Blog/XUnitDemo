using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Reflection;

namespace XUnitDemo.Tests
{
    /// <summary>
    /// 组名称: XUnitDemo.Tests
    /// 持续时间: 0:00:00.128
    /// 18 个测试失败
    /// 1 个测试跳过
    /// 4 个测试通过
    /// </summary>
    [TestFixture]
    public class AssertUnitTest
    {
        public AssertUnitTest()
        {
            System.Diagnostics.Debug.WriteLine($"This is {nameof(AssertUnitTest)}");
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
            Assert.Less(2, 1);
        }

        [Test]
        public void Multiple_ReturnVoid()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, 2);
                Assert.Less(1, 2);
                Assert.LessOrEqual(2, 1);
                Assert.IsTrue(7 < 9);
            });
        }

        [Test]
        public void Negative_Arg_ReturnVoid()
        {
            var f1 = 1.23;
            var f2 = -1.2;
            Assert.Multiple(() =>
            {
                Assert.Negative(f1);
                Assert.Negative(f2);
            });
        }

        [Test]
        public void NotNull_Object_ReturnVoid()
        {
            string obj1 = null;
            string obj2 = default(string);
            string obj3 = nameof(obj3);
            Assert.Multiple(() =>
            {
                Assert.NotNull(obj1);
                Assert.NotNull(obj2);
                Assert.NotNull(obj3);
            });
        }

        [Test]
        public void NotZero_Object_ReturnVoid()
        {
            int i1 = 0;
            int i2 = -1;
            int i3 = 5;
            Assert.Multiple(() =>
            {
                Assert.NotZero(i1);
                Assert.NotZero(i2);
                Assert.NotZero(i3);
            });
        }

        [Test]
        public void Null_Object_ReturnVoid()
        {
            string obj1 = null;
            string obj2 = default(string);
            string obj3 = nameof(obj3);
            Assert.Multiple(() =>
            {
                Assert.Null(obj1);
                Assert.Null(obj2);
                Assert.Null(obj3);
            });
        }

        [Test]
        public void Pass_ReturnVoid()
        {
            Assert.Pass();
            Assert.Pass($"{nameof(Pass_ReturnVoid)} success.");
        }

        [Test]
        public void Positive_Arg_ReturnVoid()
        {
            var f1 = 1.23;
            var f2 = -1.2;
            Assert.Multiple(() =>
            {
                Assert.Positive(f1);
                Assert.Positive(f2);
            });
        }

        [Test]
        public void ReferenceEquals_Obj1Obj2_ReturnVoid()
        {
            var str1 = "123";
            var str2 = "456";
            var str3 = "123";
            Assert.Multiple(() =>
            {
                Assert.ReferenceEquals(str1, str1);
                Assert.ReferenceEquals(str2, str2);
                Assert.ReferenceEquals(str1, str1);
            });
        }

        [Test]
        public void Throws_TypeDelegate_ReturnVoid()
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                var a = 123;
                throw new ArgumentException("Parameter a is error.");
            });

            Assert.Throws(typeof(ArgumentException), () =>
            {
                throw new Exception();
            });
        }

        [Test]
        public void That_Obj1Obj2_ReturnVoid()
        {
            var str1 = "123";
            var str2 = "123";
            var con1 = str1==str2;
            var con2 = str1.Equals(str2);
            var con3 = object.Equals(str1, str2);
            var con4 = object.Equals(str1, con3);
            Assert.Multiple(()=> {
                Assert.That(con1);
                Assert.That(con2);
                Assert.That(con3);
                Assert.That(con4);

                Assert.That<string>("123", Is.EqualTo("123"));
            });
        }

        [Test]
        public void True_Obj1Obj2_ReturnVoid()
        {
            var str1 = "123";
            var str2 = "123";
            var con1 = str1 == str2;
            var con2 = str1.Equals(str2);
            var con3 = object.Equals(str1, str2);
            var con4 = object.Equals(str1, con3);
            Assert.Multiple(() => {
                Assert.True(con1);
                Assert.True(con2);
                Assert.True(con3);
                Assert.True(con4);
            });
        }

        [Test]
        public void Warn_Obj1Obj2_ReturnVoid()
        {
            Assert.Multiple(() => {
                Assert.Warn($"{nameof(Warn_Obj1Obj2_ReturnVoid)}是一个方法。 ");
                Assert.Warn($"{nameof(Warn_Obj1Obj2_ReturnVoid)}是一个方法1。 ");
                Assert.Warn($"{nameof(Warn_Obj1Obj2_ReturnVoid)}是一个方法2。 ");
            });
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
