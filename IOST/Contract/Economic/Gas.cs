using System;
using System.Collections.Generic;
using System.Text;

namespace IOST.Contract.Economic
{
    public class Gas : Contract
    {
        public static void Pledge(Transaction tx, string creator, string name, double initialGasPledge)
        {
            tx.AddAction("gas.iost", "pledge", creator, name, string.Format("%.4f", initialGasPledge));
        }
    }
}
