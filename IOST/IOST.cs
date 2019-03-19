namespace IOST
{
    using System;
    using Newtonsoft.Json;

    public class IOST
    {
        /// <summary>
        /// Configure gas limit, gas ratio, delay, expiration
        /// </summary>
        public Options Options { get; internal set; }

        /// <summary>
        /// Used to serialize objects to JSON string
        /// </summary>
        public static Func<object, string> JSONSerializer { get; set; } = JsonConvert.SerializeObject;

        /// <summary>
        /// Used to roundoff the value of coins
        /// </summary>
        public static Func<double, double> MathRound { get; set; } = d => d;

        public IOST(Options options)
        {
            Options = options;
        }

        public Transaction Transfer(string token, string from, string to, double amount, string memo)
        {
            var tx = new Transaction(Options);

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

            var tx = new Transaction(Options);

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
