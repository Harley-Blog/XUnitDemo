using Moq;
using NUnit.Framework;

namespace XUnitDemo.NUnitTests
{
    [TestFixture]
    public class MockUnitTest
    {
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
            Assert.Multiple(()=> {
                Assert.AreEqual("演进式架构", mockBook.Object.BookName);
                Assert.AreEqual(59, mockBook.Object.Price);
            });
        }
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
}
