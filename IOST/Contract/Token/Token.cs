namespace IOST.Contract.Token
{
    public class Token : Contract
    {
        private const string CID = "token.iost";

        public static void Transfer(Transaction tx, string token, string from, string to, double amount, string memo)
        {
            amount = IOST.MathRound(amount);
            tx.AddAction(CID, "transfer", token, from, to, amount.ToString(), memo);
        }
    }
}
