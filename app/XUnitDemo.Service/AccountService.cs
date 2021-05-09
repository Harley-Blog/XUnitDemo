using System;
using System.Threading.Tasks;
using XUnitDemo.IService;

namespace XUnitDemo.Service
{
    public class AccountService:IAccountService
    {
        public AccountService()
        { }

        public async Task<bool> RegisterAccountAsync (string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(nameof(email));
            }

            return await Task.FromResult(true);
        }
    }
}
