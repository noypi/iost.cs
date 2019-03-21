namespace IOST
{
    using global::IOST.Helpers;
    using Newtonsoft.Json;
    using System;

    public class Transaction
    {
        public Options Options { get; internal set; }
        internal Rpcpb.TransactionRequest TransactionRequest { get; set; }

        public Transaction(Options options)
        {
            Options = options;
            TransactionRequest = new Rpcpb.TransactionRequest()
            {
                Time = DateHelper.UnixNano(),
                GasLimit = options.GasLimit,
                GasRatio = options.GasRatio,
                Expiration = DateHelper.UnixNano() + (options.ExpirationInMillis * 1000000),
                Delay = options.Delay,
                ChainId = options.ChainId
            };
        }

        public void AddAction(string contractID, string abi, params object[] args)
        {
            TransactionRequest.Actions.Add(NewAction(contractID, abi, IOST.JSONSerializer(args)));
        }

        public void AddApprove(string token, string amount)
        {
            TransactionRequest.AmountLimit
                              .Add(NewAmountLimit(token, amount));
        }

        public byte[] BytesForSigning()
        {
            var tx = TransactionRequest;
            return ToBytesForSigning(tx);
        }

        public static byte[] ToBytesForSigning(Rpcpb.TransactionRequest tx)
        {
            var encoder = new Helpers.SimpleEncoder(65536);
            encoder.Put(tx.Time)
                   .Put(tx.Expiration)
                   .Put(tx.GasRatio * 100)
                   .Put(tx.GasLimit * 100)
                   .Put(tx.Delay)
                   .Put(tx.ChainId)
                   .Put(new byte[] { }) // reserved
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

        protected static byte[] ActionToBytes(Rpcpb.Action action)
        {
            var se = new Helpers.SimpleEncoder(65536);
            se.Put(action.Contract)
              .Put(action.ActionName)
              .Put(action.Data);
            return se.GetBytes();
        }

        protected static byte[] AmountLimitToBytes(Rpcpb.AmountLimit amountLimit)
        {
            var se = new Helpers.SimpleEncoder(65536);
            se.Put(amountLimit.Token)
              .Put(amountLimit.Value);
            return se.GetBytes();
        }

        protected static byte[] SignatureToBytes(Rpcpb.Signature signature)
        {
            var se = new Helpers.SimpleEncoder(65536);
            se.Put((byte)signature.Algorithm)
              .Put(signature.Signature_.ToByteArray())
              .Put(signature.PublicKey.ToByteArray());
            return se.GetBytes();
        }
    }
}