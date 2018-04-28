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
using MobilWallet.Helper;
using System.IO;
using static MobilWallet.QBitNinjaJutsus.QBitNinjaJutsus;
using MobilWallet.QBitNinjaJutsus;
using static System.Console;
using System.Text;

namespace MobilWallet.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        [HttpPost]
        public UserBalance Post([FromBody]User user)
        {
            try
            {
                string email = user.Email.ToString();
                Safe safe = TransactionalMethods.DecryptWalletByAskingForPassword("/home/Blockchain/Wallet/wallet" + user.Email.ToString() + ".json", user.Password.ToString());
                string bitcoinAddress = safe.ExtKey.PrivateKey.PubKey.GetAddress(Network.TestNet).ToString();

                List<string> balances = TransactionalMethods.ShowBalance(safe);
                List<Tuple<string, string, string, string>> transactionHistory = TransactionalMethods.History(safe);
                List<TransactionHistory> historyData = new List<TransactionHistory>();
               
                foreach(var history in transactionHistory)
                {
                    historyData.Add(new TransactionHistory
                    {
                        Time = history.Item1,
                        Amount = history.Item2,
                        Confirmations = history.Item3,
                        TransactionId = history.Item4
                    }); 
                }
                
                return new UserBalance
                {
                    ConfirmedBalance = balances[0],
                    UnConfirmedBalance = balances[1],
                    history = historyData,
                    Success = true
                };

            }
            catch (Exception ex)
            {
             return new UserBalance
                {
                    ConfirmedBalance = "",
                    UnConfirmedBalance = "",
                    history = null,
                    Success = false
                };
}

        }       
    }
}
