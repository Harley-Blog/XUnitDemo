using NUnit.Framework;
using System.Threading.Tasks;
using XUnitDemo.IService;
using XUnitDemo.Service;

namespace XUnitDemo.NUnitTests.Blog
{
    [TestFixture]
    [Category("BlogService相关测试")]
    public class BlogServiceUnitTest
    {
        private IBlogService _blogService;

        [SetUp]
        public void SetUp()
        {
            _blogService = new BlogService(new StubFileManager());
        }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContent_ReturnSecurityContentAsync()
        {
            string originContent = "1111 2222 3333 4444 0000 5555 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            string targetContent = "**** **** **** **** **** **** 为了节能环保000，为了环境安全，请使用可降解垃圾袋。";
            var result = await _blogService.GetSecurityBlogAsync(originContent);
            Assert.AreEqual(result, targetContent, $"{nameof(_blogService.GetSecurityBlogAsync)} 未能正确的将内容替换为合法内容");
        }

        [Test]
        public async Task GetSecurityBlogAsync_OriginContentIsEmpty_ReturnEmptyAsync()
        {
            string originContent = "";
            var result = await _blogService.GetSecurityBlogAsync(originContent);
            Assert.AreEqual(result, string.Empty, $"{nameof(_blogService.GetSecurityBlogAsync)} 方法参数为空时，返回值也需要为空");
        }

        [TearDown]
        public void TearDown()
        {
            _blogService = null;
        }
    }
}
