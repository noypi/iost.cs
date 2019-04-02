namespace IOSTSdk
{
    using System;
    using System.IO;
    using IOSTSdk.Contract.Economic;
    using IOSTSdk.Contract.System;
    using IOSTSdk.Contract.Token;
    using Newtonsoft.Json;

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

        /// <summary>
        /// Publish new contract 
        ///    - will call the init() javascript function
        ///    - the new contract's ID is "Contract" + transaction hash
        ///      example: ContractEQ5dZ8TWGHER4CUMWDg2v6yveRrNKwnpAFEF9YjCpfQA
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="contractID"></param>
        /// <param name="abi"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Transaction PublishContract(this Transaction tx, StreamReader abi, StreamReader code)
        {
            var scode = code.ReadToEnd();
            var sabi = abi.ReadToEnd();
            
            var m = new
            {
                code = scode,
                info = JsonConvert.DeserializeObject(sabi)
            };

            string json = JsonConvert.SerializeObject(m);
            tx.SystemSetCode(json);

            return tx;
        }

        /// <summary>
        /// Updates the contract 
        ///   - will call the can_update(updateID) javascript function
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="contractID"></param>
        /// <param name="abi"></param>
        /// <param name="code"></param>
        /// <param name="updateID">
        ///     parameter passed to can_update(updateID).
        ///     can_update() is the javascript method called when trying to update.
        /// </param>
        /// <returns></returns>
        public static Transaction UpdateContract(this Transaction tx, string contractID, StreamReader abi, StreamReader code, string updateID)
        {
            var scode = code.ReadToEnd();
            var sabi = abi.ReadToEnd();

            var m = new
            {
                ID = contractID,
                code = scode,
                info = JsonConvert.DeserializeObject(sabi)
            };

            string json = JsonConvert.SerializeObject(m);
            tx.SystemUpdateCode(json, updateID);

            return tx;
        }

        /// <summary>
        /// Validates public key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
