using System.Threading.Tasks;

namespace XUnitDemo.IService
{
    public interface IBlogService
    {
        public Task<string> GetSecurityBlogAsync(string originContent);
    }
}
