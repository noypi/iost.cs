namespace IOST.Contract.Economic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Gas : Contract
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="creator"></param>
        /// <param name="name"></param>
        /// <param name="initialGasPledge">trims to 4 decimal places</param>
        public static void Pledge(Transaction tx, string creator, string name, double initialGasPledge)
        {
            tx.AddAction(Cid, "pledge", creator, name, string.Format("%.4f", initialGasPledge));
        }
    }
}
