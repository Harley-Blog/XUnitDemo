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
        #endregion

        #region 3、Mock的DefaultValueProvider用法

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
    }

    public abstract class AbstractProductService : IProductService
    {
        public abstract bool AddProduct(string name);

        public virtual bool DeleteProduct(Guid id)
        {
            return true;
        }

        public IEnumerable<ProductModel> GetProductList(string productType, double price)
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
    }

    public class ProductService : AbstractProductService
    {
        public override bool AddProduct(string name)
        {
            return DateTime.Now.Hour > 10;
        }
    }

    public class ProductModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
    }
    #endregion

    #region 3、Mock的DefaultValueProvider用法

    #endregion
}
