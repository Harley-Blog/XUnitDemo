using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XUnitDemo.Service
{
    public class AccountService
    {
        private static List<string> _accountStore;
        static AccountService()
        {
            _accountStore = new List<string>();
        }
        public AccountService()
        { }

        public bool RegisterAccount(string email)
        {
            return true;
        }
    }
}
