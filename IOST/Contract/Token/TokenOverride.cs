namespace IOSTSdk.Contract.Token
{
    public partial class Token : Contract
    {
        /// <summary>
        /// Token transfer.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="from">Token Transfer Account</param>
        /// <param name="to">Token receiving account</param>
        /// <param name="amount">round offs using IOST.MathRound</param>
        /// <param name="memo">Additional Information</param>
        public static void Transfer(Transaction tx, string tokenSym, string from, string to, double amount, string memo)
        {
            amount = IOST.MathRound(amount);
            tx.AddAction(Cid, "transfer", tokenSym, from, to, amount.ToString(), memo);
        }
    }
}
