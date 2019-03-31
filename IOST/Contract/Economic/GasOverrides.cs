using System;

namespace IOSTSdk.Contract.Economic
{
    /// <summary>
    /// 
    /// </summary>
    public static class GasExtOverrides
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="creator"></param>
        /// <param name="name"></param>
        /// <param name="initialGasPledge">trims to 4 decimal places</param>
        public static Transaction GasPledge(this Transaction tx, string creator, string name, double initialGasPledge)
        {
            var gas = IOST.MathRound(initialGasPledge).ToString();
            tx.AddAction(Gas.Cid, "pledge", creator, name, gas);
            return tx;
        }
    }
}
