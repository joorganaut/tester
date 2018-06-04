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
    public class OutwardChartModel : IBusinessObjectModel
    {
        [DataMember]
        public List<TransactionModel> Transactions { get; set; }
        [DataMember]
        public string Error { get; set; }
    }
}
