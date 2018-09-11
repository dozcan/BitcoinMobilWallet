using System;
using Microsoft.AspNetCore.Mvc;
using NBitcoin;
using System.IO;
using HBitcoin.KeyManagement;
using System.Text;
using BitcoinMobileWallet.Models;
using System.Collections;
using Microsoft.Extensions.Options;

namespace BitcoinMobileWallet.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private Mnemonic mnemonic;
        private readonly WalletDetails _walletDetails;
        private readonly IUser user;
        public RegisterController(IOptions<WalletDetails> appSettings)
        {
            _walletDetails = appSettings.Value;
        }
        public Safe CreateWallet(User user)
        {
            try
            {
                string _walletpath = Path.Combine(_walletDetails._walletPath, "wallet" + user.Email + ".json");
                Network _network;
                if (_walletDetails._network == "TEST")
                    _network = Network.TestNet;
                else _network = Network.Main;
                Safe safe = Safe.Create(out mnemonic, user.Password, _walletpath, _network); 
                return safe;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Response CreateResponse(string email,string bitcoinAddress,string mnemonicAddress)
        {
            Response _resp = new Response();
            try
            {
                RegisterResponse _regresp = new RegisterResponse();
                _resp.success = true;
                _regresp.Created = DateTime.Now;
                _regresp.Email = email;
                _regresp.BitcoinAddress = bitcoinAddress;
                _regresp.Explanation = "Registration generated succesfully";
                _regresp.MnemonicAddress = mnemonicAddress;
                _resp.ResponseObect = _regresp;
                return _resp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
         
        }
        public Response RegisterWallet(User user)
        {
            try
            {
                Safe safe = CreateWallet(user);
                string publicKey = safe.ExtKey.PrivateKey.PubKey.ToString();
                string bitcoinAddress = safe.ExtKey.PrivateKey.PubKey.GetAddress(Network.TestNet).ToString();
                string mnemonicAddress = mnemonic.ToString();
                //recover wallet from mnemonic;
                ExtKey privateKey = new ExtKey(Encoding.ASCII.GetBytes(mnemonicAddress));
                Response _resp = CreateResponse(user.Email, bitcoinAddress, mnemonicAddress);

                return _resp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }       
        // POST api/values
        [HttpPost]
        public Response Post([FromBody]User user)
        {
            try
            {
                //create keys for just test
                Key privateKey = new Key();
                PubKey publicKey = privateKey.PubKey;
                Console.WriteLine(privateKey);
                Console.WriteLine(publicKey);
                Response _resp = RegisterWallet(user);
                return _resp;
            }
            catch (Exception ex)
            {
               
                Response _resp = new Response();
                RegisterResponse _regresp = new RegisterResponse();
                _resp.success = false;
                _regresp.Created = DateTime.Now;
                _regresp.Email = user.Email;
                _regresp.Explanation = ex.Message;
                _regresp.BitcoinAddress = "";
                _regresp.MnemonicAddress = "";
                _resp.ResponseObect = _regresp;
                return _resp;
            }

        }


    }
}
