// generated by IOST.DevTools last: 3/21/2019 12:22:49 PM
namespace IOST.Contract.Token
{
    /// <summary>
    /// Token721 contract is used for the creation, distribution, transfer and destruction of non-exchangeable tokens.
    /// 
    /// Version: native
    /// Language: 1.0.0
    /// Reference: https://developers.iost.io/docs/en/6-reference/TokenContract.html
    /// </summary>
    public partial class Token721 : Contract
    {
        private const string Cid = "token721.iost";

        /// <summary>
        /// Create tokens.
        /// </summary>
        /// <param name="tokenSym">Token identifier, unique within the contract</param>
        /// <param name="issuer">issuer with issuing token rights</param>
        /// <param name="totalSupply">Total circulation, integer</param>
        public static void Create(Transaction tx, string tokenSym, string issuer, double totalSupply)
        {
            tx.AddAction(Cid, "create", tokenSym, issuer, totalSupply);
        }

        /// <summary>
        /// Issue tokens.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="to">Token receiving account</param>
        /// <param name="metaData">Meta data for tokens</param>
        public static void Issue(Transaction tx, string tokenSym, string to, string metaData)
        {
            tx.AddAction(Cid, "issue", tokenSym, to, metaData);
        }

        /// <summary>
        /// Token transfer.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="from">Token Transfer Account</param>
        /// <param name="to">Token receiving account</param>
        /// <param name="tokenID">Token ID</param>
        public static void Transfer(Transaction tx, string tokenSym, string from, string to, string tokenID)
        {
            tx.AddAction(Cid, "transfer", tokenSym, from, to, tokenID);
        }

        /// <summary>
        /// Get the token balance.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="from">Token account</param>
        public static void BalanceOf(Transaction tx, string tokenSym, string from)
        {
            tx.AddAction(Cid, "balanceOf", tokenSym, from);
        }

        /// <summary>
        /// Get the owner of a particular token
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="tokenID">Token ID</param>
        public static void OwnerOf(Transaction tx, string tokenSym, string tokenID)
        {
            tx.AddAction(Cid, "ownerOf", tokenSym, tokenID);
        }

        /// <summary>
        /// Get the index token owned by the account
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="owner">Token account</param>
        /// <param name="index">Token index, integer</param>
        public static void TokenOfOwnerByIndex(Transaction tx, string tokenSym, string owner, double index)
        {
            tx.AddAction(Cid, "tokenOfOwnerByIndex", tokenSym, owner, index);
        }

        /// <summary>
        /// Get the meta data of the token
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="tokenID">Token ID</param>
        public static void TokenMetadata(Transaction tx, string tokenSym, string tokenID)
        {
            tx.AddAction(Cid, "tokenMetadata", tokenSym, tokenID);
        }
    }
}
