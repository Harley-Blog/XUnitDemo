using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XUnitDemo.IService;

namespace XUnitDemo.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPut("security/content")]
        public async Task<string> GetSecurityBlog([FromBody]string originContent)
        {
            return await _blogService.GetSecurityBlogAsync(originContent);
        }
    }
}
