using Microsoft.Extensions.Configuration;
using NotMyDomain.Interface;
using NotMyDomain.Models;
using System.Collections.Generic;

namespace NotMyDomain
{
    internal class ApplicationOptionSelector : ConsoleOptionSelector<Application>
    {
        public ApplicationOptionSelector(IConfiguration configuration)
            : base("Select application", configuration.GetSection("applications").Get<IEnumerable<Application>>(), x => x.Name)
        {
        }
    }
}