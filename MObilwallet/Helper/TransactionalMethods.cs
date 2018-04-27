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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using static System.Console;
using HBitcoin.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using static MobilWallet.QBitNinjaJutsus.QBitNinjaJutsus;
using static System.Console;
using MobilWallet.QBitNinjaJutsus;

using static System.Console;
using System.Text;

namespace MobilWallet.Helper
{
    public class TransactionalMethods
    {
        public static List<Tuple<string, string, string, string>> History(Safe safe)
        {

            string btc = safe.ExtKey.PrivateKey.PubKey.GetAddress(Network.TestNet).ToString();
            string bitcoinAddress = safe.ExtKey.PrivateKey.PubKey.GetAddress(Network.TestNet).ToString();

            List<Tuple<string, string, string, string>> transactions;
            List<BitcoinAddress> balanceAddress = new List<BitcoinAddress>();
            Dictionary<BitcoinAddress, List<BalanceOperation>> operationsPerAddresses = new Dictionary<BitcoinAddress, List<BalanceOperation>>();
            BitcoinAddress realbtcaddres = BitcoinAddress.Create(bitcoinAddress, Network.TestNet);
            balanceAddress.Add(realbtcaddres);
            operationsPerAddresses = QueryOperationsPerAddresses(balanceAddress);

            Dictionary<uint256, List<BalanceOperation>> operationsPerTransactions = GetOperationsPerTransactions(operationsPerAddresses);

            // 3. Create history records from the transactions
            // History records is arbitrary data we want to show to the user
            var txHistoryRecords = new List<Tuple<DateTimeOffset, Money, int, uint256>>();
            var txHistoryRecordsString = new List<Tuple<string, string, string, string>>();
            foreach (var elem in operationsPerTransactions)
            {
                var amount = Money.Zero;
                foreach (var op in elem.Value)
                    amount += op.Amount;
                var firstOp = elem.Value.First();

                txHistoryRecords
                    .Add(new Tuple<DateTimeOffset, Money, int, uint256>(
                        firstOp.FirstSeen,
                        amount,
                        firstOp.Confirmations,
                        elem.Key));

                txHistoryRecordsString
                .Add(new Tuple<string, string, string, string>(
                    firstOp.FirstSeen.ToString(),
                    amount.ToString(),
                    firstOp.Confirmations.ToString(),
                    elem.Key.ToString()));
            }

            // 4. Order the records by confirmations and time (Simply time does not work, because of a QBitNinja bug)
            var orderedTxHistoryRecords = txHistoryRecords
                .OrderByDescending(x => x.Item3) // Confirmations
                .ThenBy(x => x.Item1); // FirstSeen

            return txHistoryRecordsString;
        }
        public static List<string> ShowBalance(Safe safe)
        {
            string btc;
            List<string> balances = new List<string>();
            try
            {

                btc = safe.ExtKey.PrivateKey.PubKey.GetAddress(Network.TestNet).ToString();

                BitcoinAddress addressToSend = BitcoinAddress.Create(btc, Network.TestNet);


                List<BitcoinAddress> balanceAddress = new List<BitcoinAddress>();
                Dictionary<BitcoinAddress, List<BalanceOperation>> operationsPerAddresses = new Dictionary<BitcoinAddress, List<BalanceOperation>>();
                balanceAddress.Add(addressToSend);
                operationsPerAddresses = QueryOperationsPerAddresses(balanceAddress);

                var client = new QBitNinjaClient(Network.TestNet);

                // 1. Get all address history record with a wrapper class
                var addressHistoryRecords = new List<AddressHistoryRecord>();
                foreach (var elem in operationsPerAddresses)
                {
                    foreach (var op in elem.Value)
                    {
                        addressHistoryRecords.Add(new AddressHistoryRecord(elem.Key, op));
                    }
                }

                // 2. Calculate wallet balances
                Money confirmedWalletBalance;
                Money unconfirmedWalletBalance;
                GetBalances(addressHistoryRecords, out confirmedWalletBalance, out unconfirmedWalletBalance);

                // 3. Group all address history records by addresses
                var addressHistoryRecordsPerAddresses = new Dictionary<BitcoinAddress, HashSet<AddressHistoryRecord>>();
                foreach (var address in operationsPerAddresses.Keys)
                {
                    var recs = new HashSet<AddressHistoryRecord>();
                    foreach (var record in addressHistoryRecords)
                    {
                        if (record.Address == address)
                            recs.Add(record);
                    }
                    addressHistoryRecordsPerAddresses.Add(address, recs);
                }


                foreach (var elem in addressHistoryRecordsPerAddresses)
                {
                    Money confirmedBalance;
                    Money unconfirmedBalance;
                    GetBalances(elem.Value, out confirmedBalance, out unconfirmedBalance);
                }


                balances.Add(confirmedWalletBalance.ToDecimal(MoneyUnit.BTC).ToString());
                balances.Add(unconfirmedWalletBalance.ToDecimal(MoneyUnit.BTC).ToString());
                return balances;
            }
            catch (Exception ex)
            {
                return balances;
                //return "-1" balans getirirken sorun oldu 

            }
        }

        public static void AssertCorrectMnemonicFormat(string mnemonic)
        {
            try
            {
                if (new Mnemonic(mnemonic).IsValidChecksum)
                    return;
            }
            catch (FormatException ex)
            {
                throw ex;
            }
            catch (NotSupportedException ex)
            {
                throw ex;
            }

        }

        public static Safe DecryptWalletByAskingForPassword(string walletFilePath, string password)
        {
            Safe safe = null;
            string pw;
            bool correctPw = false;
            do
            {
                try
                {
                    safe = Safe.Load(password, walletFilePath);
                    correctPw = true;
                }
                catch (System.Security.SecurityException)
                {
                    WriteLine("Invalid password, try again, (or press ctrl+c to exit):");
                    correctPw = false;
                }
            } while (!correctPw);

            if (safe == null)
                throw new Exception("Wallet could not be decrypted.");
            WriteLine($"{walletFilePath} wallet is decrypted.");
            return safe;
        }

  
    }
}
