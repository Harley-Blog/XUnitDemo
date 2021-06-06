using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace XUnitDemo.NUnitTests
{
    [TestFixture]
    public class MockUnitTest
    {
        #region  1、泛型类Mock的构造函数
        [Category("*1、泛型类Mock的构造函数*")]
        [Test]
        public void Constructor_WithParams_CheckProperty()
        {
            var mockBook = new Mock<Book>("演进式架构");
            var mockBook1 = new Mock<Book>("演进式架构", 59);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("演进式架构", mockBook.Object.BookName);
                Assert.AreEqual(0, mockBook.Object.Price);

                Assert.AreEqual("演进式架构", mockBook1.Object.BookName);
                Assert.AreEqual(59, mockBook1.Object.Price);
            });
        }

        /// <summary>
        /// Moq.MockException : IBookService.AddBook("演进式架构", 59) invocation failed with mock behavior Strict.
        /// All invocations on the mock must have a corresponding setup.
        /// </summary>
        [Category("*1、泛型类Mock的构造函数*")]
        [Test]
        public void Constructor_WithInterfaceMockBehaviorStrict_ThrowException()
        {
            var mockBookService = new Mock<IBookService>(MockBehavior.Strict);
            mockBookService.Object.AddBook("演进式架构", 59);
        }

        /// <summary>
        /// 无异常抛出
        /// </summary>
        [Category("*1、泛型类Mock的构造函数*")]
        [Test]
        public void Constructor_WithInterfaceMockBehaviorStrictAndSetup_NotThrowException()
        {
            var mockBookService = new Mock<IBookService>(MockBehavior.Strict);
            mockBookService.Setup(s => s.AddBook("演进式架构", 59)).Returns(true);
            mockBookService.Object.AddBook("演进式架构", 59);
        }

        /// <summary>
        /// 无异常抛出
        /// </summary>
        [Category("*1、泛型类Mock的构造函数*")]
        [Test]
        public void Constructor_WithClassMockBehaviorStrict_ThrowException()
        {
            var mockBookService = new Mock<BookService>(MockBehavior.Strict);
            mockBookService.Object.AddBook("演进式架构", 59);
        }

        /// <summary>
        /// 无异常抛出
        /// </summary>
        [Category("*1、泛型类Mock的构造函数*")]
        [Test]
        public void Constructor_WithClassMockBehaviorLoose_NotThrowException()
        {
            var mockBookService = new Mock<BookService>(MockBehavior.Loose);
            mockBookService.Object.AddBook("演进式架构", 59);
        }

        /// <summary>
        /// 无异常抛出
        /// </summary>
        [Category("*1、泛型类Mock的构造函数*")]
        [Test]
        public void Constructor_WithNewExpression_ReturnMockObject()
        {
            var mockBookService = new Mock<IBookService>(() => new BookService());
            mockBookService.Object.AddBook("演进式架构", 59);

            var mockBook = new Mock<Book>(() => new Book("演进式架构", 59));
            Assert.Multiple(() =>
            {
                Assert.AreEqual("演进式架构", mockBook.Object.BookName);
                Assert.AreEqual(59, mockBook.Object.Price);
            });
        }
        #endregion

        #region 2、Mock的Setup用法
        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void AddProduct_WithProductName_ReturnTrue()
        {
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.AddProduct("演进式架构")).Returns(true);

            var mockProductService1 = new Mock<AbstractProductService>();
            mockProductService1.Setup(s => s.AddProduct("演进式架构")).Returns(true);

            var mockProductService2 = new Mock<ProductService>();
            mockProductService2.Setup(s => s.AddProduct("演进式架构")).Returns(true);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(mockProductService.Object.AddProduct("演进式架构"));
                Assert.IsFalse(mockProductService.Object.AddProduct("演进式架构_59"));

                Assert.IsTrue(mockProductService1.Object.AddProduct("演进式架构"));
                Assert.IsFalse(mockProductService1.Object.AddProduct("演进式架构_59"));

                Assert.IsTrue(mockProductService2.Object.AddProduct("演进式架构"));
                Assert.IsFalse(mockProductService2.Object.AddProduct("演进式架构_59"));
            });
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void DeleteProduct_WithGuidEmpty_ThrowArgumentException()
        {
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.DeleteProduct(Guid.Empty)).Throws(new ArgumentException("id"));

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() => mockProductService.Object.DeleteProduct(Guid.Empty));
                Assert.DoesNotThrow(() => mockProductService.Object.DeleteProduct(Guid.NewGuid()));
            });
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void GetProudctName_WithGuid_ReturnParamAsResult()
        {
            var mockProductService = new Mock<IProductService>();
            var id = Guid.Empty;
            mockProductService.Setup(s => s.GetProudctName(It.IsAny<Guid>()))
                .Returns<Guid>(s => s.ToString() + "123");

            var a = mockProductService.Object.GetProudctName(Guid.Empty);
            Assert.AreEqual(a, $"{Guid.Empty.ToString()}123");
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void GetProudctName_WithGuid_ReturnEmptyWithAOP()
        {
            var mockProductService = new Mock<IProductService>();
            var id = Guid.Empty;
            mockProductService.Setup(s => s.GetProudctName(It.IsAny<Guid>()))
                .Callback<Guid>(s => System.Diagnostics.Debug.WriteLine($"Before Invoke GetProudctName; param:{s.ToString()}"))
                .Returns<Guid>(s => string.Empty)
                .Callback<Guid>(s => System.Diagnostics.Debug.WriteLine($"After Invoke GetProudctName; param:{s.ToString()}"));

            mockProductService.Object.GetProudctName(Guid.Empty);
            Assert.Pass();
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void GetProductList_WithProductTypePriceInRange_ReturnProductList()
        {
            var mockProductService = new Mock<IProductService>();
            var item = new ProductModel()
            {
                ProductId = Guid.NewGuid(),
                ProductName = "演进式架构",
                ProductPrice = 59
            };
            mockProductService.Setup(s => s.GetProductList(It.Is<string>(a => a == "Book"), It.IsInRange<double>(20, 60, Moq.Range.Inclusive)))
                .Returns(new List<ProductModel> { item });

            var productService = mockProductService.Object;
            var result = productService.GetProductList("Book", 59);
            var result1 = productService.GetProductList("Books", 59);
            var result2 = productService.GetProductList("Book", 5);
            Assert.Multiple(() =>
            {
                CollectionAssert.AreEqual(new List<ProductModel>() { item }, result);
                CollectionAssert.AreEqual(new List<ProductModel>() { item }, result1, "param:bookType=Books,price=59返回的result1与预期的不相符");
                CollectionAssert.AreEqual(new List<ProductModel>() { item }, result2, "param:bookType=Book,price=5返回的result2与预期的不相符");
            });
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void GetProductList_WithProductTypeRegexPriceIsAny_ReturnProductList()
        {
            var mockProductService = new Mock<IProductService>();
            var item = new ProductModel()
            {
                ProductId = Guid.NewGuid(),
                ProductName = "演进式架构",
                ProductPrice = 59
            };
            mockProductService.Setup(s => s.GetProductList(It.IsRegex("^[1-9a-z_]{1,10}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase), It.IsAny<double>()))
                .Returns(new List<ProductModel> { item });

            var productService = mockProductService.Object;
            var result = productService.GetProductList("Book_123", 59);
            var result1 = productService.GetProductList("BookBookBookBookBook", 123);
            var result2 = productService.GetProductList("书籍", 5);
            Assert.Multiple(() =>
            {
                CollectionAssert.AreEqual(new List<ProductModel>() { item }, result);
                CollectionAssert.AreEqual(new List<ProductModel>() { item }, result1, "param:bookType=BookBookBookBookBook,price=123返回的result1与预期的不相符");
                CollectionAssert.AreEqual(new List<ProductModel>() { item }, result2, "param:bookType=书籍,price=5返回的result2与预期的不相符");
            });
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void Repaire_WithIdIsAny_TriggerMyHandlerEvent()
        {
            var mockProductService = new Mock<IProductService>();
            var id = Guid.NewGuid();
            mockProductService.Setup(s => s.Repaire(It.IsAny<Guid>())).Raises<Guid>((s) =>
            {
                s.MyHandlerEvent += null;//这个注册的委托不会被调用，实际上是触发ProductServiceHandler中的Invoke委托
            }, s => new MyEventArgs(id));

            var myHandler = new ProductServiceHandler(mockProductService.Object);
            System.Diagnostics.Debug.WriteLine($"This is {nameof(Repaire_WithIdIsAny_TriggerMyHandlerEvent)} Id={id}");
            mockProductService.Object.Repaire(id);
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void GetProductList_WithProductTypePrice_VerifyTwice()
        {
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.GetProductList(It.IsAny<string>(), It.IsAny<double>()))
                .Returns(new List<ProductModel>())
                .Verifiable();
            var result = mockProductService.Object.GetProductList("Book", 59);
            result = mockProductService.Object.GetProductList("Books", 5);

            mockProductService.Verify(s => s.GetProductList(It.IsAny<string>(), It.IsAny<double>()), Times.AtLeast(2), "GetProductList 没有按照预期执行2次");
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void GetProductList_WithProductTypePrice_MockVerify()
        {
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.GetProductList(It.IsAny<string>(), It.IsAny<double>()))
                .Returns(new List<ProductModel>())
                .Verifiable("GetProductList没有按照预期执行一次");
            var result = mockProductService.Object.GetProductList("Book", 59);
            Mock.Verify(mockProductService);
        }

        [Category("*2、Mock的Setup用法*")]
        [Test]
        public void GetProductList_WithProductTypePrice_MockVerifyAll()
        {
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.GetProductList(It.IsAny<string>(), It.IsAny<double>()))
                .Returns(new List<ProductModel>());

            var mockAbstractProductService = new Mock<AbstractProductService>();
            mockAbstractProductService.Setup(s => s.GetProductList(It.IsAny<string>(), It.IsAny<double>()))
                .Returns(new List<ProductModel>());

            mockProductService.Object.GetProductList("Book", 59);
            mockAbstractProductService.Object.GetProductList("Book", 59);
            Mock.VerifyAll(mockProductService, mockAbstractProductService);
        }
        #endregion

        #region 3、Mock的CallBase用法
        [Category("*3、Mock的CallBase用法*")]
        [Test]
        public void GetProductName_WithIProductServiceAnyGuidCallBaseIsTrue_ReturnEmpty()
        {
            var mockProductService = new Mock<IProductService>() { CallBase = true };
            mockProductService.Setup(s => s.GetProudctName(It.IsAny<Guid>())).Returns(string.Empty);
            var result = mockProductService.Object.GetProudctName(Guid.NewGuid());
            Assert.AreEqual(string.Empty, result);
        }

        [Category("*3、Mock的CallBase用法*")]
        [Test]
        public void GetProductName_WithAbstractProductServiceAnyGuidCallBaseIsTrue_ReturnBaseResult()
        {
            var mockProductService = new Mock<AbstractProductService>() { };
            mockProductService.Setup(s => s.GetProudctName(It.IsAny<Guid>()));
            var result = mockProductService.Object.GetProudctName(Guid.NewGuid());
            Assert.AreEqual("演进式架构", result);
        }

        [Category("*3、Mock的CallBase用法*")]
        [Test]
        public void ToCurrentString_WithProductServiceCallBaseIsTrue_ReturnBaseResult()
        {
            var mockProductService = new Mock<ProductService>();
            //mockProductService.Setup(s => s.ToCurrentString()).Returns(string.Empty);
            var result = mockProductService.Object.ToCurrentString();
            Assert.AreEqual("ProductService_ToString", result);
        }

        #endregion

        #region 4、Mock的DefaultValue用法
        [Category("*4、Mock的DefaultValue用法*")]
        [Test]
        public void MockDefaultValue_WithDefaultValueMock_ReturnExpect()
        {
            var mockRolePermissionService = new Mock<IRolePermissionService>() { DefaultValue = DefaultValue.Mock };
            var roleRepos = mockRolePermissionService.Object.RoleRepository;
            var permissionRepos = mockRolePermissionService.Object.PermissionRepository;
            Assert.Multiple(() =>
            {
                Assert.NotNull(roleRepos);
                Assert.NotNull(permissionRepos);
                Assert.NotNull(roleRepos.DbContext);
                Assert.NotNull(permissionRepos.DbContext);
            });
        }

        [Category("*4、Mock的DefaultValue用法*")]
        [Test]
        public void GetRoleName_WidthRolePermissionServiceDefaultValueMock_ReturnExpect()
        {
            var mockRolePermissionService = new Mock<IRolePermissionService>() { DefaultValue = DefaultValue.Mock };
            var roleRepos = mockRolePermissionService.Object.RoleRepository;
            var mockRoleRepos = Mock.Get(roleRepos);
            mockRoleRepos.Setup(s => s.GetRoleName(It.IsAny<Guid>())).Returns("Admin");
            var result = mockRolePermissionService.Object.RoleRepository.GetRoleName(Guid.NewGuid());

            Assert.AreEqual("Admin", result);
        }
        #endregion

        #region 5、SetupProperty与SetupAllProperties的用法
        [Category("*5、SetupProperty与SetupAllProperties的用法*")]
        [Test]
        public void CheckProperty_WithSetupProperty_ShouldPass()
        {
            var mockCar = new Mock<ICar>();
            mockCar.SetupProperty(s => s.CarBrand).SetupProperty(s => s.CarModel);
            mockCar.SetupProperty(s => s.CarBrand, "一汽大众")
                .SetupProperty(s => s.CarModel, "七座SUV");

            mockCar.Object.CarBrand = "上汽大众";
            mockCar.Object.CarModel = "五座SUV";
            Assert.Multiple(() =>
            {
                Assert.AreEqual("七座SUV", mockCar.Object.CarModel);
                Assert.AreEqual("一汽大众", mockCar.Object.CarBrand);
            });
        }

        [Category("*5、SetupProperty与SetupAllProperties的用法*")]
        [Test]
        public void CheckProperty_WithSetupAllProperties_ShouldPass()
        {
            var mockCar = new Mock<ICar>();
            mockCar.SetupAllProperties();

            mockCar.Object.CarBrand = "上汽大众";
            mockCar.Object.CarModel = "五座SUV";
            Assert.Multiple(() =>
            {
                Assert.AreEqual("七座SUV", mockCar.Object.CarModel);
                Assert.AreEqual("一汽大众", mockCar.Object.CarBrand);
            });
        }

        [Category("*5、SetupProperty与SetupAllProperties的用法*")]
        [Test]
        public void CheckProperty_WithSetupSetVerifySet_ShouldPass()
        {
            var mockCar = new Mock<ICar>();
            mockCar.SetupSet(s => s.CarBrand ="上汽大众");
            mockCar.SetupSet(s => s.CarModel = "五座SUV");

            mockCar.Object.CarBrand = "上汽大众";
            mockCar.Object.CarModel = "五座SUV";
            
            //mockCar.Object.CarBrand = "一汽大众";
            //mockCar.Object.CarModel = "七座SUV";

            mockCar.VerifySet(s => s.CarBrand = "上汽大众"); ;
            mockCar.VerifySet(s => s.CarModel = "五座SUV"); ;
        }
        #endregion

        #region 6、Mock的As用法
        [Category("*6、Mock的As用法*")]
        [Test]
        public void MockAs_WithMultipleInterface_ShouldPass()
        {
            var mockComputer = new Mock<IComputer>();

            var mockKeyBoard = mockComputer.As<IKeyboard>();
            var mockScreen = mockComputer.As<IScreen>();
            var mockCpu = mockComputer.As<ICpu>();
            mockKeyBoard.Setup(s => s.GetKeyboardType()).Returns("机械键盘");
            mockScreen.Setup(s => s.GetScreenType()).Returns("OLED");
            mockCpu.Setup(s => s.GetCpuType()).Returns("Intel-11代I7");
            var keyboardType = ((dynamic)mockComputer.Object).GetKeyboardType();
            var screenType = ((dynamic)mockComputer.Object).GetScreenType();
            var cpuType = ((dynamic)mockComputer.Object).GetCpuType();

            Assert.Multiple(() =>
            {
                Assert.AreEqual("机械键盘", keyboardType);
                Assert.AreEqual("OLED", screenType);
                Assert.AreEqual("Intel-11代I7", cpuType);
            });
        }

        [Category("*6、Mock的As用法*")]
        [Test]
        public void MockAs_WithMultipleInterfaceAndInvokePropertyBeforeAs_ShouldPass()
        {
            var mockComputer = new Mock<IComputer>();
            mockComputer.Setup(s => s.ComputerType).Returns("台式机");
            var computerType = mockComputer.Object.ComputerType;

            var mockKeyBoard = mockComputer.As<IKeyboard>();
            var mockScreen = mockComputer.As<IScreen>();
            var mockCpu = mockComputer.As<ICpu>();
            mockKeyBoard.Setup(s => s.GetKeyboardType()).Returns("机械键盘");
            mockScreen.Setup(s => s.GetScreenType()).Returns("OLED");
            mockCpu.Setup(s => s.GetCpuType()).Returns("Intel-11代I7");
            var keyboardType = ((dynamic)mockComputer.Object).GetKeyboardType();
            var screenType = ((dynamic)mockComputer.Object).GetScreenType();
            var cpuType = ((dynamic)mockComputer.Object).GetCpuType();

            Assert.Multiple(() =>
            {
                Assert.AreEqual("台式机", computerType);
                Assert.AreEqual("机械键盘", keyboardType);
                Assert.AreEqual("OLED", screenType);
                Assert.AreEqual("Intel-11代I7", cpuType);
            });
        }

        #endregion

        #region 7、Mock的异步方法设置
        [Category("*7、Mock的异步方法设置*")]
        [Test]
        public async Task AddProductAsync_WithName_ReturnTrue()
        {
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.AddProductAsync(It.IsAny<string>()).Result).Returns(true);
            //mockProductService.Setup(s => s.AddProductAsync(It.IsAny<string>())).ReturnsAsync(true);

            var result = await mockProductService.Object.AddProductAsync("演进式架构");
            Assert.AreEqual(true, result);
        }
        #endregion

        #region 8、Mock的Sequence用法
        [Category("*8、Mock的Sequence用法*")]
        [Test]
        public void GetProductName_WithId_ReturnDifferenctValueInMultipleInvoke()
        {
            var mockProductService = new Mock<IProductService>();
            mockProductService.SetupSequence(s => s.GetProudctName(It.IsAny<Guid>()))
                .Returns("渐进式架构")
                .Returns("Vue3实战")
                .Returns("Docker实战")
                .Returns("微服务架构设计模式");

            var result = mockProductService.Object.GetProudctName(Guid.Empty);
            var result1 = mockProductService.Object.GetProudctName(Guid.Empty);
            var result2 = mockProductService.Object.GetProudctName(Guid.Empty);
            var result3 = mockProductService.Object.GetProudctName(Guid.Empty);
            Assert.Multiple(() =>
            {
                Assert.AreEqual("渐进式架构", result);
                Assert.AreEqual("Vue3实战", result1);
                Assert.AreEqual("Docker实战", result2);
                Assert.AreEqual("微服务架构设计模式", result3);
            });
        }

        [Category("*8、Mock的Sequence用法*")]
        [Test]
        public void InSequence_WithMultipleNonSequenceInvoke_ThrowException()
        {
            var mockProductService = new Mock<IProductService>(MockBehavior.Strict);
            var sequence = new MockSequence();
            mockProductService.InSequence(sequence)
                .Setup(s => s.AddProductAsync(It.IsAny<string>())).ReturnsAsync(true);
            mockProductService.InSequence(sequence)
                .Setup(s => s.GetProudctName(It.IsAny<Guid>())).Returns("渐进式架构");
            mockProductService.InSequence(sequence)
                .Setup(s => s.DeleteProduct(It.IsAny<Guid>())).Returns(true);

            mockProductService.Object.AddProductAsync("渐进式架构");
            mockProductService.Object.DeleteProduct(Guid.Empty);
            mockProductService.Object.GetProudctName(Guid.Empty);
        }

        [Category("*8、Mock的Sequence用法*")]
        [Test]
        public void InSequence_WithMultipleInSequenceInvoke_WillPass()
        {
            var mockProductService = new Mock<IProductService>(MockBehavior.Strict);
            var sequence = new MockSequence();
            mockProductService.InSequence(sequence)
                .Setup(s => s.AddProductAsync(It.IsAny<string>())).ReturnsAsync(true);
            mockProductService.InSequence(sequence)
                .Setup(s => s.GetProudctName(It.IsAny<Guid>())).Returns("渐进式架构");
            mockProductService.InSequence(sequence)
                .Setup(s => s.DeleteProduct(It.IsAny<Guid>())).Returns(true);

            mockProductService.Object.AddProductAsync("渐进式架构");
            mockProductService.Object.GetProudctName(Guid.Empty);
            mockProductService.Object.DeleteProduct(Guid.Empty);
        }
        #endregion

        #region 9、Mock的Protected用法

        [Category("*9、Mock的Protected用法*")]
        [Test]
        public void Calculate_WithProtectedMembers_CanAccessProtectedMembers()
        {
            var mockCalculator = new Mock<Calculator>(12, 10, 100, 5);
            mockCalculator.Protected().Setup<double>("GetPercent").Returns(0.5);
            mockCalculator.Protected().Setup<double>("GetSalt", ItExpr.IsAny<double>()).Returns(0.9);

            var obj = mockCalculator.Object;
            var sum = obj.Sum();
            var division = obj.Division();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(22 * 0.5 * 0.9, sum);
                Assert.AreEqual(100 * 0.5 * 0.9 / 5, division);
            });
        }

        [Category("*9、Mock的Protected用法*")]
        [Test]
        public void Calculate_WithProtectedMembersUnRelatedInterface_CanAccessProtectedMembers()
        {
            var mockCalculator = new Mock<Calculator>(12, 10, 100, 5);
            var caculatorProtectedMembers = mockCalculator.Protected().As<ICaculatorProtectedMembers>();
            caculatorProtectedMembers.Setup(s => s.GetPercent()).Returns(0.6);
            caculatorProtectedMembers.Setup(s => s.GetSalt(It.IsAny<double>())).Returns(0.8);

            var obj = mockCalculator.Object;
            var sum = obj.Sum();
            var division = obj.Division();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(22 * 0.6 * 0.8, sum);
                Assert.AreEqual(100 * 0.6 * 0.8 / 5, division);
            });
        }
        #endregion

        #region 10、Mock的Of用法
        [Category("*10、Mock的Of用法*")]
        [Test]
        public void MockOf_WithLinq_QuickSetup()
        {
            var context = Mock.Of<HttpContext>(hc =>
              hc.User.Identity.IsAuthenticated == true &&
              hc.User.Identity.Name == "harley" &&
              hc.Response.ContentType == "application/json" &&
              hc.RequestServices == Mock.Of<IServiceProvider>
              (a => a.GetService(typeof(ICaculatorProtectedMembers)) == Mock.Of<ICaculatorProtectedMembers>
             (p => p.GetPercent() == 0.2 && p.GetSalt(It.IsAny<double>()) == 0.3)));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(true, context.User.Identity.IsAuthenticated);
                Assert.AreEqual("harley", context.User.Identity.Name);
                Assert.AreEqual("application/json", context.Response.ContentType);
                Assert.AreEqual(0.2, context.RequestServices.GetService<ICaculatorProtectedMembers>().GetPercent());
                Assert.AreEqual(0.3, context.RequestServices.GetService<ICaculatorProtectedMembers>().GetSalt(1));
                ;
            });
        }
        #endregion

        #region 11、Mock泛型参数进行匹配
        [Category("*10、Mock的Of用法*")]
        [Test]
        public void SaveJsonToString_TObject_ReturnTrue()
        {
            var mockRedisService = new Mock<IRedisService>();
            mockRedisService.Setup(s => s.SaveJsonToString(It.IsAny<It.IsAnyType>()))
                .Returns(true);
            var kv = new KeyValuePair<string, string>("Harley", "Coder");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(true, mockRedisService.Object.SaveJsonToString(kv));
                Assert.AreEqual(true, mockRedisService.Object.SaveJsonToString(new { Name = "Harley", JobType = "Coder" }));
            });
        }

        [Category("*10、Mock的Of用法*")]
        [Test]
        public void SavePersonToString_WithMale_ReturnTrue()
        {
            var mockRedisService = new Mock<IRedisService>();
            var male = new Male { Name = "Harley" };
            mockRedisService.Setup(s => s.SavePersonToString(It.IsAny<It.IsSubtype<Person>>())).Returns(true);
            var result1 = mockRedisService.Object.SavePersonToString(new { Name = "Harley", JobType = "Coder" });
            var result2 = mockRedisService.Object.SavePersonToString(new Person { Name = "Harley" });
            var result3 = mockRedisService.Object.SavePersonToString(new Male { Name = "Harley" });
            Assert.Multiple(() =>
            {
                Assert.AreEqual(true, result1,"使用匿名类型生成的对象无法通过测试");
                Assert.AreEqual(true, result2,"使用Person类型生成的对象无法通过测试");
                Assert.AreEqual(true, result3, "使用Male类型生成的对象无法通过测试");
            });
        }
        #endregion
    }

    #region 1、泛型类Mock的构造函数
    public class Book
    {
        public string BookName;
        public double Price;
        public Book()
        {
        }

        public Book(string bookName)
        {
            BookName = bookName;
        }

        public Book(string bookName, double price)
        {
            BookName = bookName;
            Price = price;
        }
    }

    public interface IBookService
    {
        public bool AddBook(string bookName, double price);
    }

    public class BookService : IBookService
    {
        public bool AddBook(string bookName, double price)
        {
            return true;
        }
    }
    #endregion

    #region 2、Mock的Setup用法
    public interface IProductService
    {
        public bool AddProduct(string name);
        public Task<bool> AddProductAsync(string name);
        public bool DeleteProduct(Guid id);
        public string GetProudctName(Guid id);
        public IEnumerable<ProductModel> GetProductList(string productType, double price);
        public event EventHandler MyHandlerEvent;
        public void Repaire(Guid id);
    }

    public abstract class AbstractProductService : IProductService
    {
        public event EventHandler MyHandlerEvent;

        public abstract bool AddProduct(string name);

        public async Task<bool> AddProductAsync(string name)
        {
            return await Task.FromResult(true);
        }

        public virtual bool DeleteProduct(Guid id)
        {
            return true;
        }

        public virtual IEnumerable<ProductModel> GetProductList(string productType, double price)
        {
            return new List<ProductModel> {
                new ProductModel(){
                        ProductId=Guid.NewGuid(),
                        ProductName="演进式架构",
                        ProductPrice=59
                 }
            };
        }

        public virtual string GetProudctName(Guid id)
        {
            return "演进式架构";
        }

        public void Repaire(Guid id)
        {

        }
    }

    public class ProductService : AbstractProductService
    {
        public override bool AddProduct(string name)
        {
            return DateTime.Now.Hour > 10;
        }

        public override string GetProudctName(Guid id)
        {
            return base.GetProudctName(id);
        }

        public string ToCurrentString()
        {
            return $"{ nameof(ProductService)}_{nameof(ToString)}";
        }
    }

    public class ProductModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
    }

    public class MyEventArgs : EventArgs
    {
        public Guid Id { get; set; }
        public MyEventArgs(Guid id)
        {
            Id = id;
        }
    }

    public class ProductServiceHandler
    {
        private IProductService _productService;
        public ProductServiceHandler(IProductService productService)
        {
            _productService = productService;
            _productService.MyHandlerEvent += Invoke;
        }

        public void Invoke(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine($"This is {nameof(ProductServiceHandler)} {nameof(Invoke)} Id={((MyEventArgs)args).Id}");
        }
    }
    #endregion

    #region 4、Mock的DefaultValue属性
    public interface IRolePermissionService
    {
        public IRoleRepository RoleRepository { get; set; }
        public IPermissionRepository PermissionRepository { get; set; }
        public IEnumerable<string> GetPermissionList(Guid roleId);
    }
    public interface IRoleRepository
    {
        public IDbContext DbContext { get; set; }
        public string GetRoleName(Guid roleId);
    }

    public interface IPermissionRepository
    {
        public IDbContext DbContext { get; set; }
        public string GetPermissionName(Guid permissionId);
    }

    public interface IDbContext
    {

    }
    #endregion

    #region 5、SetupProperty与SetupAllProperties的用法
    public interface ICar
    {
        public IEnumerable<IWheel> Wheels { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
    }
    public interface IWheel
    {
        public string WheelHub { get; set; }
        public string WheelTyre { get; set; }
        public string WheelTyreTube { get; set; }
    }

    public class Car : ICar
    {
        public IEnumerable<IWheel> Wheels { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
    }

    public class CarWheel : IWheel
    {
        public string WheelHub { get; set; }
        public string WheelTyre { get; set; }
        public string WheelTyreTube { get; set; }
    }
    #endregion

    #region 6、Mock的As用法
    public interface IComputer
    {
        public string ComputerType { get; set; }
    }

    public interface IScreen
    {
        public string GetScreenType();
    }

    public interface IMainBoard
    {
        public ICpu GetCpu();
    }

    public interface IKeyboard
    {
        public string GetKeyboardType();
    }

    public interface ICpu
    {
        public string GetCpuType();
    }
    #endregion

    #region 9、Mock的Protected用法

    public class Calculator
    {
        public Calculator(int first, int second, double number, double divisor)
        {
            First = first;
            Second = second;
            Number = number;
            Divisor = divisor;
        }
        protected int First { get; set; }
        protected int Second { get; set; }
        protected double Number { get; set; }
        protected double Divisor { get; set; }
        public double Sum()
        {
            return (First + Second) * GetPercent() * GetSalt(0.9);
        }
        public double Division()
        {
            return Number * GetPercent() * GetSalt(0.9) / Divisor;
        }

        protected virtual double GetPercent()
        {
            return 0.9;
        }
        protected virtual double GetSalt(double salt)
        {
            return salt;
        }
    }

    public interface ICaculatorProtectedMembers
    {
        double GetPercent();
        double GetSalt(double salt);
    }
    #endregion

    #region 11、Mock的类型匹配
    public interface IRedisService
    {
        public bool SaveJsonToString<T>(T TObject);
        public bool SavePersonToString<T>(T TObject);
    }
    public class Person
    {
        public string Name { get; set; }
    }

    public class Male : Person
    {

    }
    #endregion
}
