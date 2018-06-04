using NIP.Adhoc.Core.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIP.Adhoc.Core.data
{
    [Serializable]

    public class Transaction : IBusinessObject
    {
        public DateTime DateCreated { get; set; }
        public string ResponseCode { get; set; }
        public string AccountNo { get; set; }
        public string SessionID { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string Channel { get; set; }
        public long ID { get; set; }
        public int Status { get; set; }
    }
}
