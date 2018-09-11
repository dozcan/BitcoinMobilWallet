using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBitcoin.KeyManagement;

namespace BitcoinMobileWallet.Models
{

    public class Response
    {
        public bool success { get; set; }
        public IResponse ResponseObect { get; set; }

    }

    public interface IResponse  
    {
        DateTime Created { get; set; }
        string Explanation { get; set; }
    }

    public class LoginResponse : UserBalance, IResponse
    {
        private DateTime _Created;
        private string _Email;
        private string _Explanation;
        public DateTime Created { get { return _Created; } set { _Created = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }

        public string Explanation { get { return _Explanation; } set { _Explanation = value; } }
    }

    public class RegisterResponse : BitcoinRegister,IResponse
    {
        private DateTime _Created;
        private string _Email;
        private string _Explanation;
        public DateTime Created { get { return _Created; } set { _Created = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }
        public string Explanation { get { return _Explanation; } set { _Explanation = value; } }
    }

}

    




