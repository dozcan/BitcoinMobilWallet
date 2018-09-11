using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMobileWallet.Models
{
    public class BtcTransactionDetail
    {
        public string BtcSenderAddress { get; set; }
        public string SendBtcAmount { get; set; }
        public string BtcReceiverAddres { get; set; }
        public User _user { get; set; }
    }
    public interface IUser
    {
         string Email { get; set; }
         string Password { get; set; }
    }

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
   
    public class RecoverUser
    {
        public string Mnemonic { get; set; }
        public string Password { get; set; }
    }
}
