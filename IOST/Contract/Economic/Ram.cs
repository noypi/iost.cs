using System;
using System.Collections.Generic;
using System.Text;

namespace IOST.Contract.Economic
{
    public class Ram : Contract
    {
        public static void Buy(Transaction tx, string creator, string name, long initialRAM)
        {
            tx.AddAction("ram.iost", "buy", creator, name, initialRAM);
        }
    }
}
