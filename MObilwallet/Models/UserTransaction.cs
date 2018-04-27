using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilWallet.Models
{
    public class UserBalance
    {
        public string ConfirmedBalance { get; set; }
        public string UnConfirmedBalance { get; set; }
        public Boolean Success { get; set; }
        public IEnumerable<TransactionHistory> history { get; set; }
    }


    public class TransactionHistory
    {
        public string Time { get; set; }
        public string Amount { get; set; }
        public string Confirmations { get; set; }
        public string TransactionId { get; set; }
    }
}
