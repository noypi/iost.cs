namespace IOSTSdk
{
    using System.Threading.Tasks;

    public partial class IOST
    {
        /// <summary>
        /// Configure gas limit, gas ratio, delay, expiration
        /// </summary>
        public Options Options { get; private set; }

        private Client _client;

        /// <summary>
        /// Create a new IOST
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        public IOST(Client client, Options options)
        {
            Options = options;
            _client = client;
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <returns></returns>
        public Transaction NewTransaction() => new Transaction(Options);

        /// <summary>
        /// Sends the transaction
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="keychain"></param>
        /// <returns>the transaction hash</returns>
        public Task<string> Send(Transaction tx)
        {
            return _client.SendTransaction(tx.TransactionRequest)
                          .ContinueWith<string>((task) => 
                           {
                               if (task.Exception != null)
                               {
                                   throw task.Exception;
                               }
                               return task.Result?.Hash;
                           });
        }
    }
}
