namespace IOST.Contract.Economic
{
    public class Ram : Contract
    {
        private const string CID = "ram.iost";

        public static void Buy(Transaction tx, string creator, string name, long initialRAM)
        {
            tx.AddAction(CID, "buy", creator, name, initialRAM);
        }
    }
}
