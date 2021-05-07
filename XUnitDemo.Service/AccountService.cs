using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XUnitDemo.Service
{
    public class AccountService
    {
        public AccountService()
        { }

        public bool RegisterAccount(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(nameof(email));
            }

            return true;
        }
    }
}
