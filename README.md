# MobilWallet
1) this backend of deterministic bitcoin wallet which use hbitcoin and nbitcoin 
2) it uses pay to public key hash(P2PKH) 
   private key => public key => hash = Bitcoin address(add network) <=> pubsub key for bitcoin receiver for out transaction(ownership)
   satoishi decided 2 level of hashing for pubsub key because of solving private key from public key(quantum computing)
3) receiver => bitcoinaddress.ScriptPubKey
4) you can get login, register, show balance, show history and you send btc.

Requests
/*******************************************************************
http://localhost:49678/api/Login
REQUEST =>
{
	"Email":"doga@gmail.com",
	"Password":"1111"
}
RESPONSE =>
{
    "success": true,
    "responseObect": {
        "created": "2018-09-11T14:32:10.8246943+03:00",
        "email": "doga@gmail.com",
        "explanation": "login successfully generated",
        "confirmedBalance": "0",
        "unConfirmedBalance": "0",
        "history": []
    }
}
********************************************************************/
