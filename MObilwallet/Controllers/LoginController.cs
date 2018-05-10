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
        public Response Post([FromBody]User user)
        {
            try
            {
                string email = user.Email.ToString();
                Safe safe = TransactionalMethods.DecryptWalletByAskingForPassword("c:/Blockchain/Wallet/wallet" + user.Email.ToString() + ".json", user.Password.ToString());
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

                Response _resp = new Response();
                LoginResponse _logresp = new LoginResponse();
                _logresp.Created = DateTime.Now;
                _logresp.Email = email;
                _logresp.ConfirmedBalance = balances[0];
                _logresp.UnConfirmedBalance = balances[1];
                _logresp.history = historyData;
                _logresp.Explanation = "login successfully generated";
                _resp.success = true;
                _resp.ResponseObect = _logresp;
                

                return _resp;

            }
            catch (Exception ex)
            {
                Response _resp = new Response();
                LoginResponse _logresp = new LoginResponse();
                _logresp.Created = DateTime.Now;
                _logresp.Email = "";
                _logresp.ConfirmedBalance = "";
                _logresp.UnConfirmedBalance = "";
                _logresp.Explanation = ex.Message;
                _logresp.history = null;
                _resp.success = false;
                _resp.ResponseObect = _logresp;

                return _resp;

            }

        }       
    }
}
