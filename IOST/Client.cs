namespace IOSTSdk
{
    using Grpc.Core;
    using Rpcpb;
    using System.Threading;
    using System.Threading.Tasks;

    public class Client : ApiService.ApiServiceClient
    {
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
        public Client(string url) : base(new Channel(url, ChannelCredentials.Insecure))
        {
        }

        public Client(Channel channel) : base(channel)
        {
        }

        public Task<NodeInfoResponse> GetNodeInfo()
        {
            return GetNodeInfoAsync(new EmptyRequest(), new CallOptions(null, null, default(CancellationToken)))
                       .ResponseAsync;
        }

        public Task<ChainInfoResponse> GetChainInfo()
        {
            return GetChainInfoAsync(new EmptyRequest(), new CallOptions(null, null, default(CancellationToken)))
                       .ResponseAsync;
        }

        public Task<RAMInfoResponse> GetRamInfo()
        {
            return GetRAMInfoAsync(new EmptyRequest(), new CallOptions(null, null, default(CancellationToken)))
                       .ResponseAsync;
        }

        public Task<GasRatioResponse> GetGasRatio()
        {
            return GetGasRatioAsync(new EmptyRequest(), new CallOptions(null, null, default(CancellationToken)))
                       .ResponseAsync;
        }
    }
}
