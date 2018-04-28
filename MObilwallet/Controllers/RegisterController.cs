using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MobilWallet.Models;
using NBitcoin;
using Newtonsoft.Json.Linq;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using static System.Console;
using HBitcoin.KeyManagement;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using static MobilWallet.QBitNinjaJutsus.QBitNinjaJutsus;
using MobilWallet.QBitNinjaJutsus;
using static System.Console;
using System.Text;

namespace MobilWallet.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        // POST api/values
        [HttpPost]
        public NewAddress Post([FromBody]User user)
        {
            try
            {
                //bu dosyalar, mysql docker ile paketlenip iki ayrı sunucuda replice edilmeli
                string walletpath = Path.Combine("/home/Blockchain/Wallet", "wallet" + user.Email.ToString() + ".json");

                Mnemonic mnemonic;
                Safe safe = Safe.Create(out mnemonic, user.Password, walletpath, Network.TestNet);
                string publicKey = safe.ExtKey.PrivateKey.PubKey.ToString();
                string bitcoinAddress = safe.ExtKey.PrivateKey.PubKey.GetAddress(Network.TestNet).ToString();
                string mnemonicAddress = mnemonic.ToString();

                ExtKey privateKey = new ExtKey(Encoding.ASCII.GetBytes(mnemonicAddress));

                return new NewAddress { BitcoinAddress = bitcoinAddress, MnemonicAddress = mnemonicAddress,Success=true };

            }
            catch (Exception ex)
            {
                return new NewAddress { BitcoinAddress = "", MnemonicAddress = "" ,Success=false};
            }

        }


    }
}
