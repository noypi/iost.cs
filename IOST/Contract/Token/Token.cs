using System;
using System.Collections.Generic;
using System.Text;

namespace IOST.Contract.Token
{
    public class Token : Contract
    {
        public static void Transfer(Transaction tx, string token, string from, string to, double amount, string memo)
        {
            amount = IOST.MathRound(amount);
            tx.AddAction("token.iost", "transfer", token, from, to, amount.ToString(), memo);
        }
    }
}
