namespace IOST.Contract.Economic
{
    public class Gas : Contract
    {
        private const string CID = "gas.iost";

        public static void Pledge(Transaction tx, string creator, string name, double initialGasPledge)
        {
            tx.AddAction(CID, "pledge", creator, name, string.Format("%.4f", initialGasPledge));
        }
    }
}
