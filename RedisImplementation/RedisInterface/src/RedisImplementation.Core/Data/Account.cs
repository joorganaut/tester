
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisImplementation.Core.Data
{
    public class Account
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string KYCLevel { get; set; }
        public AccountStatus Status { get; set; }
    }
    public enum AccountStatus
    {
        Active = 0,
        Inactive = 1,
        Dormant = 2
    }
}
