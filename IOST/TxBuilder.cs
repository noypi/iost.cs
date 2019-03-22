using System;
using System.Collections.Generic;
using System.Text;

namespace IOST
{
    public class TxBuilder
    {
        private readonly Options _options;

        internal TxBuilder(Options options)
        {
            _options = options;
        }

        public Transaction Transfer(string token, string from, string to, double amount, string memo)
        {
            var tx = new Transaction(_options);

            Contract.Token.Token.Transfer(tx, token, from, to, amount, memo);
            tx.AddApprove("iost", amount.ToString());

            return tx;
        }

        public Transaction NewAccount(string name, string creator, string ownerkey, string activekey,
                                        int initialRAM, double initialGasPledge)
        {
            if (!ValidatePubKey(ownerkey) || !ValidatePubKey(activekey))
            {
                throw new ArgumentException("invalid public key");
            }

            var tx = new Transaction(_options);

            Contract.System.Auth.SignUp(tx, name, ownerkey, activekey);
            Contract.Economic.Ram.Buy(tx, creator, name, initialRAM);
            Contract.Economic.Gas.Pledge(tx, creator, name, initialGasPledge);

            return tx;
        }

        protected bool ValidatePubKey(string key)
        {
            bool bRet = false;
            try
            {
                byte[] k = IOST.Base58Decode(key);
                bRet = (k.Length == 32);
            }
            catch(Exception)
            {
                bRet = false;
            }
            return bRet;
        }
    }
}
