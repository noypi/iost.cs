namespace IOSTSdk
{
    using global::IOSTSdk.Helpers;
    using System.Linq;
    using System.Text;

    public class Transaction
    {
        internal Transaction(Options options)
        {
            Options = options;
            TxRequest = new Rpcpb.TransactionRequest()
            {
                Time = DateHelper.UnixNano(),
                GasLimit = options.GasLimit,
                GasRatio = options.GasRatio,
                Expiration = DateHelper.UnixNano() + (options.ExpirationInMillis * 1000000),
                Delay = options.Delay,
                ChainId = options.ChainId
            };
        }

        public Options Options { get; internal set; }

        public Rpcpb.TransactionRequest TxRequest { get; set; }

        public void AddAction(string contractID, string abi, params object[] args)
        {
            TxRequest.Actions.Add(NewAction(contractID, abi, IOST.JSONSerializer(args)));
        }

        public void AddApprove(string token, string amount)
        {
            TxRequest.AmountLimit
                              .Add(NewAmountLimit(token, amount));
        }

        public byte[] BytesForSigning()
        {
            var tx = TxRequest;
            return ToBytesForSigning(tx);
        }

        public static byte[] ToBytesForSigning(Rpcpb.TransactionRequest tx, Encoding textEncoding = null)
        {
            textEncoding = textEncoding ?? IOST.DefaultTextEncoding;

            var encoder = new Helpers.SimpleEncoder(65536)
            {
                TextEncoding = textEncoding
            };
            encoder.Put(tx.Time)
                   .Put(tx.Expiration)
                   .Put((long)(tx.GasRatio * 100))
                   .Put((long)(tx.GasLimit * 100))
                   .Put(tx.Delay)
                   .Put(tx.ChainId)
                   .Put((byte[])null) // reserved
                   .Put(tx.Signers)
                   .PutList<Rpcpb.Action>(tx.Actions, ActionToBytes)
                   .PutList<Rpcpb.AmountLimit>(tx.AmountLimit, AmountLimitToBytes)
                   .PutList<Rpcpb.Signature>(tx.Signatures, SignatureToBytes);

            return encoder.GetBytes();
        }

        public static Rpcpb.Action NewAction(string contract, string name, string data)
        {
            return new Rpcpb.Action()
            {
                Contract = contract,
                ActionName = name,
                Data = data
            };
        }

        public static Rpcpb.AmountLimit NewAmountLimit(string token, string value)
        {
            return new Rpcpb.AmountLimit()
            {
                Token = token,
                Value = value
            };
        }

        public static byte[] ActionToBytes(Rpcpb.Action action, Encoding encoding)
        {
            var se = new Helpers.SimpleEncoder(65536)
            {
                TextEncoding = encoding
            };
            se.Put(action.Contract)
              .Put(action.ActionName)
              .Put(action.Data);
            return se.GetBytes();
        }

        public static byte[] AmountLimitToBytes(Rpcpb.AmountLimit amountLimit, Encoding encoding)
        {
            var se = new Helpers.SimpleEncoder(65536)
            {
                TextEncoding = encoding
            };
            se.Put(amountLimit.Token)
              .Put(amountLimit.Value);
            return se.GetBytes();
        }

        public static byte[] SignatureToBytes(Rpcpb.Signature signature, Encoding encoding)
        {
            var se = new Helpers.SimpleEncoder(65536)
            {
                TextEncoding = encoding
            };
            se.Put((byte)signature.Algorithm)
              .Put(signature.Signature_.ToByteArray())
              .Put(signature.PublicKey.ToByteArray());
            return se.GetBytes();
        }

#if false // decouple ChainPay
        public static readonly string SRTRANSFERMETHOD_MUSTREPLY = SignatureRequest<Rpcpb.TransactionRequest>.TRANSFERMETHOD_MUSTREPLY;

        public static readonly string SRTRANSFERMETHOD_CLIENTCANTRANFER = SignatureRequest<Rpcpb.TransactionRequest>.TRANSFERMETHOD_CLIENTCANTRANSFER;

        public string CreateSignatureRequest(string tag, string transferMethod)
        {
            var tx = new SignatureRequest<Rpcpb.TransactionRequest>()
            {
                BlockchainCode = "IOST",
                BlockchainName = "Internet of Services Token",
                MessageHash = Convert.ToBase64String(IOST.CryptoHashSha3_256(BytesForSigning())),
                HashAlgo = "SHA3-256",
                Tag = tag,
                TransferDetails = TransactionRequest,
                TransferMethod = transferMethod
            };
            
            return ChainPay.CreateSignatureRequest<Rpcpb.TransactionRequest>(tx);
        }

        public static SignatureRequest<Rpcpb.TransactionRequest> ReadSignatureRequest(string sig)
        {
            return ChainPay.ReadSignatureRequest<Rpcpb.TransactionRequest>(sig);
        }

        public static SignatureRequest<Rpcpb.TransactionRequest> ReadSignatureRequest(StreamReader reader)
        {
            return ChainPay.ReadSignatureRequest<Rpcpb.TransactionRequest>(reader);
        }

        public string CreateSignatureResponse(SignatureResponse response)
        {
            response.BlockchainCode = "IOST";
            response.BlockchainName = "Internet of Services Token";

            return ChainPay.CreateSignatureResponse(response);
        }

        public static SignatureResponse ReadSignatureResponse(StreamReader reader)
        {
            return ChainPay.ReadSignatureResponse(reader);
        }
#endif
    }
}