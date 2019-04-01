namespace IOSTSdk
{
    using System;
    using IOSTSdk.Contract.Economic;
    using IOSTSdk.Contract.System;
    using IOSTSdk.Contract.Token;

    public static class TxBuilderExt
    {
        /// <summary>
        /// Transfer a token to an account
        /// </summary>
        /// <param name="token">the token to transfer, example: "iost"</param>
        /// <param name="from">the source account name</param>
        /// <param name="to">the destination account name</param>
        /// <param name="amount">the amount to transfer</param>
        /// <param name="memo">maximum of 512 characters</param>
        /// <returns></returns>
        public static Transaction Transfer(this Transaction tx, string token, string from, string to, double amount, string memo)
        {
            tx.TokenTransfer(token, from, to, amount, memo);
            tx.AddApprove("iost", amount.ToString());

            return tx;
        }

        /// <summary>
        /// Vote for a Servi Candidate
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="accountName"></param>
        /// <param name="candidateName"></param>
        /// <param name="numberOfVotes"></param>
        /// <returns></returns>
        public static Transaction VoteAServiCandidate(this Transaction tx, string accountName, string candidateName, string numberOfVotes)
        {
            return tx.VoteProducerVote(accountName, candidateName, numberOfVotes);
        }

        /// <summary>
        /// Unvote for a Servi Candidate
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="accountName"></param>
        /// <param name="candidateName"></param>
        /// <param name="numberOfVotes"></param>
        /// <returns></returns>
        public static Transaction UnvoteAServiCandidate(this Transaction tx, string accountName, string candidateName, string numberOfVotes)
        {
            return tx.VoteProducerUnvote(accountName, candidateName, numberOfVotes);
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="name">the account</param>
        /// <param name="creator">the one who pays for the new account</param>
        /// <param name="ownerkey">the public key with the 'owner' permission</param>
        /// <param name="activekey">the public key with the 'active' permission</param>
        /// <param name="initialGasPledge"></param>
        /// <param name="initialRAM"></param>
        /// <returns></returns>
        public static Transaction CreateAccount(this Transaction tx, string name, string creator, string ownerkey, string activekey,
                                      double initialGasPledge = 1, int initialRAM = 1024)
        {
            if (!ValidatePubKey(ownerkey) || !ValidatePubKey(activekey))
            {
                throw new ArgumentException("invalid public key");
            }

            tx.AuthSignUp(name, ownerkey, activekey)
              .RamBuy(creator, name, initialRAM);

            if (!IsZero(initialGasPledge))
            {
                tx.GasPledge(creator, name, initialGasPledge);
            }

            return tx;
        }

        static bool ValidatePubKey(string key)
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

        static bool IsZero(double d)
        {
            return (Math.Abs(d) < 0.00000001);
        }
    }
}
