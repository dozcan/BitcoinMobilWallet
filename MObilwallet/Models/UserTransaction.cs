using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMobileWallet.Models
{
    public class BitcoinRegister
    {
        public string BitcoinAddress { get; set; }
        public string MnemonicAddress { get; set;}
    }
       
    public class UserBalance
    {
        public string ConfirmedBalance { get; set; }
        public string UnConfirmedBalance { get; set; }
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
