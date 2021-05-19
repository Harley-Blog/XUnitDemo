using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using XUnitDemo.IService;

namespace XUnitDemo.Tests
{
    [TestFixture]
    public class RoleServiceUnitTest
    {
        [SetUp]
        public void SetUp()
        { }

        #region Constructor
        public void Counstructor_Test()
        { 
            
        }
        #endregion

        #region 对参数进行设置
        public async Task AddRoleAsync_WithMockBehaviorStrict()
        {
            Mock mock = new Mock<IRoleService>(MockBehavior.Strict);
        }
        #endregion

        [TearDown]
        public void TearDown()
        { }
    }
}
