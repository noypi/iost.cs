using System;
using System.Collections.Generic;
using System.Text;

namespace IOST
{
    public class TxBuilder
    {
        private Options _options;

        internal TxBuilder(Options options)
        {
            _options = options;
        }

        public Transaction Transfer(string token, string from, string to, double amount, string memo)
        {
            var tx = new Transaction(_options);

            Contract.Token.Token.Transfer(tx, token, from, to, amount, memo);
            tx.AddApprove("iost", amount);

            return tx;
        }

        public Transaction NewAccount(string name, string creator, string ownerkey, string activekey,
                                        long initialRAM, double initialGasPledge)
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
            return false;
        }
    }
}
