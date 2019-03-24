namespace IOSTSdk.Contract.Token
{
    public static class TokenExtOverrides
    {
        /// <summary>
        /// Token transfer.
        /// </summary>
        /// <param name="tokenSym">Token Identifier</param>
        /// <param name="from">Token Transfer Account</param>
        /// <param name="to">Token receiving account</param>
        /// <param name="amount">round offs using IOST.MathRound</param>
        /// <param name="memo">Additional Information</param>
        public static Transaction TokenTransfer(this Transaction tx, string tokenSym, string from, string to, double amount, string memo)
        {
            amount = IOST.MathRound(amount);
            tx.AddAction(Token.Cid, "transfer", tokenSym, from, to, amount.ToString(), memo);
            return tx;
        }
    }
}
