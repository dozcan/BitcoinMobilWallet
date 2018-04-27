using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilWallet.Models
{
    public class NewAddress
    {
        public string BitcoinAddress { get; set; }
        public string MnemonicAddress { get; set; }

        public Boolean Success { get; set; }
    }
}
