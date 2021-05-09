using System.Threading.Tasks;

namespace XUnitDemo.IService
{
    public interface IAccountService
    {
        public Task<bool> RegisterAccountAsync(string email);
    }
}
