namespace IOSTSdk
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Rpcpb;

    public class Client : IDisposable
    {
        private ApiServerClient _asc = null;

        public static Client NewAustrialia() => new Client(Channels.NewAustrialia());
        public static Client NewCanada()     => new Client(Channels.NewCanada());
        public static Client NewFrance()     => new Client(Channels.NewFrance());
        public static Client NewGermany()    => new Client(Channels.NewGermany());
        public static Client NewJapan()      => new Client(Channels.NewJapan());
        public static Client NewKorea()      => new Client(Channels.NewKorea());
        public static Client NewUK()         => new Client(Channels.NewUK());
        public static Client NewUS()         => new Client(Channels.NewUS());

        /// <summary>
        /// Testnet Blockchain explorer - http://54.249.186.224/
        /// Create a test account- http://54.249.186.224/applyIOST
        /// API - http://13.52.105.102:30001
        /// </summary>
        /// <returns></returns>
        public static Client NewTestNet() => new Client(Channels.NewTestNet());

        /// <summary>
        /// Creates a new IOST from a url
        /// </summary>
        /// <param name="url">Example: localhost:30002</param>
        public Client(string url) : this(new Channel(url, ChannelCredentials.Insecure))
        {
        }

        public Client(Channel channel)
        {
            _asc = new ApiServerClient(channel);
        }

        public Task<NodeInfoResponse> GetNodeInfo()
        {
            return _asc.GetNodeInfo(new CallOptions(null, null, default(CancellationToken)))
                       .ResponseAsync;
        }

        public Task<ChainInfoResponse> GetChainInfo()
        {
            return _asc.GetChainInfo(new CallOptions { })
                       .ResponseAsync;
        }

        public Task<TransactionResponse> GetTxByHash(TxHashRequest request)
        {
            return _asc.GetTxByHash(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<TxReceipt> GetTxReceiptByTxHash(TxHashRequest request)
        {
            return _asc.GetTxReceiptByTxHash(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<BlockResponse> GetBlockByHash(GetBlockByHashRequest request)
        {
            return _asc.GetBlockByHash(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<BlockResponse> GetBlockByNumber(GetBlockByNumberRequest request)
        {
            return _asc.GetBlockByNumber(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<Account> GetAccount(GetAccountRequest request)
        {
            return _asc.GetAccount(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<GetTokenBalanceResponse> GetTokenBalance(GetTokenBalanceRequest request)
        {
            return _asc.GetTokenBalance(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<Rpcpb.Contract> GetContract(GetContractRequest request)
        {
            return _asc.GetContract(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<GetContractStorageResponse> GetContractStorage(GetContractStorageRequest request)
        {
            return _asc.GetContractStorage(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<GetContractStorageFieldsResponse> GetContractStorageFields(GetContractStorageFieldsRequest request)
        {
            return _asc.GetContractStorageFields(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<GetProducerVoteInfoResponse> GetProducerVoteInfo(GetProducerVoteInfoRequest request)
        {
            return _asc.GetProducerVoteInfo(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<CandidateBonus> GetCandidateBonus(GetAccountRequest request)
        {
            return _asc.GetCandidateBonus(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<VoterBonus> GetVoterBonus(GetAccountRequest request)
        {
            return _asc.GetVoterBonus(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<SendTransactionResponse> SendTransaction(TransactionRequest request)
        {
            return _asc.SendTransaction(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<TxReceipt> ExecTransaction(TransactionRequest request)
        {
            return _asc.ExecTransaction(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<SubscribeResponse> Subscribe(SubscribeRequest request)
        {
            return _asc.Subscribe(request, new CallOptions { })
                       .ResponseAsync;
        }

        public Task<TokenInfo> GetTokenInfo(GetTokenInfoRequest request)
        {
            return _asc.GetTokenInfo(request, new CallOptions { })
                       .ResponseAsync;
        }

        public void Dispose()
        {
            _asc?.Dispose();
        }
    }
}
