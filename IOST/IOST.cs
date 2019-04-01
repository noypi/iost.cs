namespace IOSTSdk
{
    using System;
    using System.Threading.Tasks;
    using IOSTSdk.Crypto;

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
        public IOST(Client client, Options options = null)
        {
            Options = options ?? new Options();
            _client = client;
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <returns></returns>
        public Transaction NewTransaction() => new Transaction(Options.Clone());

        /// <summary>
        /// Sends the transaction
        /// </summary>
        /// <param name="tx"></param>
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

                               if ((task.Result.PreTxReceipt != null) &&
                                   (task.Result.PreTxReceipt.StatusCode != Rpcpb.TxReceipt.Types.StatusCode.Success))
                               {
                                   throw new InvalidOperationException(task.Result.PreTxReceipt.Message);
                               }
                               
                               return task.Result?.Hash;
                           });
        }

        /// <summary>
        /// Executes the transaction
        /// </summary>
        /// <param name="tx"></param>
        /// <returns>the transaction hash</returns>
        public Task<Rpcpb.TxReceipt> Execute(Transaction tx)
        {
            return _client.ExecTransaction(tx.TransactionRequest);
        }

        /// <summary>
        /// Generate a new keypair
        /// </summary>
        /// <returns>the new keypair</returns>
        public KeyPair GenerateNewKeyPair()
        {
            return SodiumEd25519.NewKeyPair();
        }
    }
}
