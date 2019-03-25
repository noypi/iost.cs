// generated by IOST.DevTools last: 3/26/2019 5:52:37 AM
namespace IOSTSdk.Contract.Token
{
     using IOSTSdk;

    /// <summary>
    /// Token721 contract is used for the creation, distribution, transfer and destruction of non-exchangeable tokens.
    /// 
    /// Version: native
    /// Language: 1.0.0
    /// Reference: https://developers.iost.io/docs/en/6-reference/TokenContract.html
    /// </summary>
    public static class Token721
    {
        public static readonly string Cid = "token721.iost";

        /// <summary>
        /// Create tokens.
        /// </summary>
        /// <param name="tokenSym">Token identifier, unique within the contract</param>
        /// <param name="issuer">issuer with issuing token rights</param>
        /// <param name="totalSupply">Total circulation, integer</param>
        public static Transaction Token721Create(this Transaction tx, string tokenSym, string issuer, int totalSupply)
        {
            tx.AddAction(Cid, "create", tokenSym, issuer, totalSupply);
            return tx;
        }

        /// <summary>
        /// Issue tokens.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="to">Token receiving account</param>
        /// <param name="metaData">Meta data for tokens</param>
        public static Transaction Token721Issue(this Transaction tx, string tokenSym, string to, string metaData)
        {
            tx.AddAction(Cid, "issue", tokenSym, to, metaData);
            return tx;
        }

        /// <summary>
        /// Token transfer.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="from">Token Transfer Account</param>
        /// <param name="to">Token receiving account</param>
        /// <param name="tokenID">Token ID</param>
        public static Transaction Token721Transfer(this Transaction tx, string tokenSym, string from, string to, string tokenID)
        {
            tx.AddAction(Cid, "transfer", tokenSym, from, to, tokenID);
            return tx;
        }

        /// <summary>
        /// Get the token balance.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="from">Token account</param>
        public static Transaction Token721BalanceOf(this Transaction tx, string tokenSym, string from)
        {
            tx.AddAction(Cid, "balanceOf", tokenSym, from);
            return tx;
        }

        /// <summary>
        /// Get the owner of a particular token
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="tokenID">Token ID</param>
        public static Transaction Token721OwnerOf(this Transaction tx, string tokenSym, string tokenID)
        {
            tx.AddAction(Cid, "ownerOf", tokenSym, tokenID);
            return tx;
        }

        /// <summary>
        /// Get the index token owned by the account
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="owner">Token account</param>
        /// <param name="index">Token index, integer</param>
        public static Transaction Token721TokenOfOwnerByIndex(this Transaction tx, string tokenSym, string owner, int index)
        {
            tx.AddAction(Cid, "tokenOfOwnerByIndex", tokenSym, owner, index);
            return tx;
        }

        /// <summary>
        /// Get the meta data of the token
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="tokenID">Token ID</param>
        public static Transaction Token721TokenMetadata(this Transaction tx, string tokenSym, string tokenID)
        {
            tx.AddAction(Cid, "tokenMetadata", tokenSym, tokenID);
            return tx;
        }
    }
}
