namespace IOST
{
    using System;
    using Google.Protobuf;
    using Grpc.Core;
    using Rpcpb;

    public class ApiServerClient : ClientBase<ApiServerClient>, IDisposable
    {
        // Methods
        private static readonly
            Func<Method<EmptyRequest, NodeInfoResponse>> __Method_GetNodeInfo = () => new Method<EmptyRequest, NodeInfoResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetNodeInfo",
                                                                                                __Marshaller_EmptyRequest(),
                                                                                                __Marshaller_NodeInfoResponse());

        private static readonly
            Func<Method<EmptyRequest, ChainInfoResponse>> __Method_GetChainInfo = () => new Method<EmptyRequest, ChainInfoResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetChainInfo",
                                                                                                __Marshaller_EmptyRequest(),
                                                                                                __Marshaller_ChainInfoResponse());
        private static readonly
            Func<Method<TxHashRequest, TransactionResponse>> __Method_GetTxByHash = () => new Method<TxHashRequest, TransactionResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetTxByHash",
                                                                                                __Marshaller_TxHashRequest(),
                                                                                                __Marshaller_TransactionResponse());
        private static readonly
            Func<Method<TxHashRequest, TxReceipt>> __Method_GetTxReceiptByTxHash = () => new Method<TxHashRequest, TxReceipt>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetTxReceiptByTxHash",
                                                                                                __Marshaller_TxHashRequest(),
                                                                                                __Marshaller_TxReceipt());
        private static readonly
            Func<Method<GetBlockByHashRequest, BlockResponse>> __Method_GetBlockByHash = () => new Method<GetBlockByHashRequest, BlockResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetBlockByHash",
                                                                                                __Marshaller_GetBlockByHashRequest(),
                                                                                                __Marshaller_BlockResponse());
        private static readonly
            Func<Method<GetBlockByNumberRequest, BlockResponse>> __Method_GetBlockByNumber = () => new Method<GetBlockByNumberRequest, BlockResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetBlockByNumber",
                                                                                                __Marshaller_GetBlockByNumberRequest(),
                                                                                                __Marshaller_BlockResponse());

        private static readonly
            Func<Method<GetAccountRequest, Account>> __Method_GetAccount = () => new Method<GetAccountRequest, Account>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetAccount",
                                                                                                __Marshaller_GetAccountRequest(),
                                                                                                __Marshaller_Account());
        private static readonly
            Func<Method<GetTokenBalanceRequest, GetTokenBalanceResponse>> __Method_GetTokenBalance = () => new Method<GetTokenBalanceRequest, GetTokenBalanceResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetTokenBalance",
                                                                                                __Marshaller_GetTokenBalanceRequest(),
                                                                                                __Marshaller_GetTokenBalanceResponse());
        private static readonly
            Func<Method<GetContractRequest, Rpcpb.Contract>> __Method_GetContract = () => new Method<GetContractRequest, Rpcpb.Contract>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetContract",
                                                                                                __Marshaller_GetContractRequest(),
                                                                                                __Marshaller_Contract());
        private static readonly
            Func<Method<GetContractStorageRequest, GetContractStorageResponse>> __Method_GetContractStorage = () => new Method<GetContractStorageRequest, GetContractStorageResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "GetContractStorage",
                                                                                                __Marshaller_GetContractStorageRequest(),
                                                                                                __Marshaller_GetContractStorageResponse());
        private static readonly
            Func<Method<TransactionRequest, SendTransactionResponse>> __Method_SendTransaction = () => new Method<TransactionRequest, SendTransactionResponse>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "SendTransaction",
                                                                                                __Marshaller_TransactionRequest(),
                                                                                                __Marshaller_SendTransactionResponse());
        private static readonly
            Func<Method<TransactionRequest, TxReceipt>> __Method_ExecTransaction = () => new Method<TransactionRequest, TxReceipt>(
                                                                                                MethodType.Unary,
                                                                                                "rpcpb.ApiService",
                                                                                                "ExecTransaction",
                                                                                                __Marshaller_TransactionRequest(),
                                                                                                __Marshaller_TxReceipt());

        // Marshallers

        // requests
        private static readonly 
            Func<Marshaller<EmptyRequest>> __Marshaller_EmptyRequest = () => Marshallers.Create<EmptyRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg), 
                                                                                                EmptyRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<TxHashRequest>> __Marshaller_TxHashRequest = () => Marshallers.Create<TxHashRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                TxHashRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetBlockByHashRequest>> __Marshaller_GetBlockByHashRequest = () => Marshallers.Create<GetBlockByHashRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                GetBlockByHashRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetBlockByNumberRequest>> __Marshaller_GetBlockByNumberRequest = () => Marshallers.Create<GetBlockByNumberRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                GetBlockByNumberRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetAccountRequest>> __Marshaller_GetAccountRequest = () => Marshallers.Create<GetAccountRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                GetAccountRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetTokenBalanceRequest>> __Marshaller_GetTokenBalanceRequest = () => Marshallers.Create<GetTokenBalanceRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                GetTokenBalanceRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetContractRequest>> __Marshaller_GetContractRequest = () => Marshallers.Create<GetContractRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                GetContractRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetContractStorageRequest>> __Marshaller_GetContractStorageRequest = () => Marshallers.Create<GetContractStorageRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                GetContractStorageRequest.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<TransactionRequest>> __Marshaller_TransactionRequest = () => Marshallers.Create<TransactionRequest>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                TransactionRequest.Parser.ParseFrom);

        // responses
        private static readonly
            Func<Marshaller<NodeInfoResponse>> __Marshaller_NodeInfoResponse = () => Marshallers.Create<NodeInfoResponse>(
                                                                                                arg => MessageExtensions.ToByteArray(arg), 
                                                                                                NodeInfoResponse.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<ChainInfoResponse>> __Marshaller_ChainInfoResponse = () => Marshallers.Create<ChainInfoResponse>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                ChainInfoResponse.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<TransactionResponse>> __Marshaller_TransactionResponse = () => Marshallers.Create<TransactionResponse>(
                                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                                TransactionResponse.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<TxReceipt>> __Marshaller_TxReceipt = () => Marshallers.Create<TxReceipt>(
                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                TxReceipt.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<BlockResponse>> __Marshaller_BlockResponse = () => Marshallers.Create<BlockResponse>(
                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                BlockResponse.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<Account>> __Marshaller_Account = () => Marshallers.Create<Account>(
                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                Account.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetTokenBalanceResponse>> __Marshaller_GetTokenBalanceResponse = () => Marshallers.Create<GetTokenBalanceResponse>(
                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                GetTokenBalanceResponse.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<Rpcpb.Contract>> __Marshaller_Contract = () => Marshallers.Create<Rpcpb.Contract>(
                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                Rpcpb.Contract.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<GetContractStorageResponse>> __Marshaller_GetContractStorageResponse = () => Marshallers.Create<GetContractStorageResponse>(
                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                GetContractStorageResponse.Parser.ParseFrom);
        private static readonly
            Func<Marshaller<SendTransactionResponse>> __Marshaller_SendTransactionResponse = () => Marshallers.Create<SendTransactionResponse>(
                                                                                arg => MessageExtensions.ToByteArray(arg),
                                                                                SendTransactionResponse.Parser.ParseFrom);

        private readonly string _host = null;
        private Channel _channel = null;

        public ApiServerClient(CallInvoker callInvoker) : base(callInvoker)
        {
        }

        public ApiServerClient(Channel channel) : base(channel)
        {
            _channel = channel;
        }

        protected ApiServerClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        public static ApiServerClient Connect(string host, int port)
        {
            var channel = new Channel(host, port, ChannelCredentials.Insecure);
            return new ApiServerClient(channel);
        }

        public static ApiServerClient Connects(string host, int port, SslCredentials ssl)
        {
            var channel = new Channel(host, port, ssl ?? new SslCredentials());
            return new ApiServerClient(channel);
        }

        public AsyncUnaryCall<NodeInfoResponse> GetNodeInfo(CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<EmptyRequest, NodeInfoResponse>(__Method_GetNodeInfo(), _host, options, new EmptyRequest());
        }

        public AsyncUnaryCall<ChainInfoResponse> GetChainInfo(CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<EmptyRequest, ChainInfoResponse>(__Method_GetChainInfo(), _host, options, new EmptyRequest());
        }

        public AsyncUnaryCall<TransactionResponse> GetTxByHash(TxHashRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<TxHashRequest, TransactionResponse>(__Method_GetTxByHash(), _host, options, request);
        }

        public AsyncUnaryCall<TxReceipt> GetTxReceiptByTxHash(TxHashRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<TxHashRequest, TxReceipt>(__Method_GetTxReceiptByTxHash(), _host, options, request);
        }

        public AsyncUnaryCall<BlockResponse> GetBlockByHash(GetBlockByHashRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<GetBlockByHashRequest, BlockResponse>(__Method_GetBlockByHash(), _host, options, request);
        }

        public AsyncUnaryCall<BlockResponse> GetBlockByNumber(GetBlockByNumberRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<GetBlockByNumberRequest, BlockResponse>(__Method_GetBlockByNumber(), _host, options, request);
        }

        public AsyncUnaryCall<Account> GetAccount(GetAccountRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<GetAccountRequest, Account>(__Method_GetAccount(), _host, options, request);
        }

        public AsyncUnaryCall<GetTokenBalanceResponse> GetTokenBalance(GetTokenBalanceRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<GetTokenBalanceRequest, GetTokenBalanceResponse>(__Method_GetTokenBalance(), _host, options, request);
        }

        public AsyncUnaryCall<Rpcpb.Contract> GetContract(GetContractRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<GetContractRequest, Rpcpb.Contract>(__Method_GetContract(), _host, options, request);
        }

        public AsyncUnaryCall<GetContractStorageResponse> GetContractStorage(GetContractStorageRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<GetContractStorageRequest, GetContractStorageResponse>(__Method_GetContractStorage(), _host, options, request);
        }

        public AsyncUnaryCall<SendTransactionResponse> SendTransaction(TransactionRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<TransactionRequest, SendTransactionResponse>(__Method_SendTransaction(), _host, options, request);
        }

        public AsyncUnaryCall<TxReceipt> ExecTransaction(TransactionRequest request, CallOptions options)
        {
            return CallInvoker.AsyncUnaryCall<TransactionRequest, TxReceipt>(__Method_ExecTransaction(), _host, options, request);
        }

        protected override ApiServerClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new ApiServerClient(configuration);
        }

        public void Dispose()
        {
            _channel?.ShutdownAsync();
            _channel = null;
        }
    }
}
