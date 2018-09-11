using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NBitcoin;
using Newtonsoft.Json.Linq;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using HBitcoin.KeyManagement;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using BitcoinMobileWallet.Helper;
using static BitcoinMobileWallet.QBitNinjaJutsus.QBitNinjaJutsus;
using BitcoinMobileWallet.QBitNinjaJutsus;
using static System.Console;
using BitcoinMobileWallet.Models;
using Microsoft.Extensions.Options;


namespace BitcoinMobileWallet.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly WalletDetails _walletDetails;
        public LoginController(IOptions<WalletDetails> appSettings)
        {
            _walletDetails = appSettings.Value;
        }

        [HttpPost]
        public Response Post([FromBody]User user)
        {
            try
            {
                string email = user.Email.ToString();
                string walletpath = Path.Combine(_walletDetails._walletPath, "wallet" + user.Email.ToString() + ".json");

                Safe safe = TransactionalMethods.DecryptWalletByAskingForPassword(walletpath, user.Password.ToString());
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
