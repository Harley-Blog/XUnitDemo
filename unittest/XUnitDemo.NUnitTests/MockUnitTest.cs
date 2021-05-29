using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

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
}
