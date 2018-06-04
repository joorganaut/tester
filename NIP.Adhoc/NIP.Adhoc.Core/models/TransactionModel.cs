using NIP.Adhoc.Core.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NIP.Adhoc.Core.models
{
    [Serializable]
    public class TransactionModel : IBusinessObjectModel
    {
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public TransactionDirection Direction { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public TransactionStatus Status { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string Error { get; set; }
    }

    [Serializable]
    public enum TransactionDirection
    {
        Inward = 0,
        Outward = 1
    }
    [Serializable]
    public enum TransactionStatus
    {
        Pending = 0,
        Processed = 1,
        FraudCheck = 2,
        Failed = 3, 
        Reversed = 4,
        Blank = 5
    }
}
