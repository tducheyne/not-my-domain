using Microsoft.Extensions.Configuration;
using NotMyDomain.Interface;
using NotMyDomain.Models;
using System.Collections.Generic;

namespace NotMyDomain
{
    internal class AccountOptionSelector : ConsoleOptionSelector<Account>
    {
        public AccountOptionSelector(IConfiguration configuration)
            : base("Select account", configuration.GetSection("accounts").Get<IEnumerable<Account>>(), x => x.Username)
        {
        }
    }
}