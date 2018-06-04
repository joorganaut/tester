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
    public class ChartModel : IBusinessObjectModel
    {
        //public string 
        [DataMember]
        public List<TransactionModel> IncomingTransactions { get; set; }
        [DataMember]
        public List<TransactionModel> OutgoingTransactions { get; set; }
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public TransactionDirection Direction { get; set; }
        public ChartModel()
        {
            IncomingTransactions = new List<TransactionModel>();
            OutgoingTransactions = new List<TransactionModel>();
        }
    }
}
