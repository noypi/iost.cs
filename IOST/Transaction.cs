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
                Delay = options.Delay
            };
        }

        public void AddAction(string contractID, string abi, params object[] args)
        {
            TransactionRequest.Actions.Add(NewAction(contractID, abi, IOST.JSONSerializer(args)));
        }

        public void AddApprove(string token, double amount)
        {
            TransactionRequest.AmountLimit.Add(
                    new Rpcpb.AmountLimit()
                    {
                        Token = token,
                        Value = amount
                    });
        }

        public byte[] BytesForSigning()
        {
            var tx = TransactionRequest;
            var encoder = new Helpers.SimpleEncoder(65536);
            encoder.Put(tx.Time)
                   .Put(tx.Expiration)
                   .Put(tx.GasRatio * 100)
                   .Put(tx.GasLimit * 100)
                   .Put(tx.Delay)
                   .Put(tx.Signers);

            var actionBytes = new byte[tx.Actions.Count][];
            for(int i=0; i<tx.Actions.Count; i++)
            {
                actionBytes[i] = ActionToBytes(tx.Actions[i]);
            }
            encoder.Put(actionBytes);

            var amountBytes = new byte[tx.AmountLimit.Count][];
            for (int i = 0; i < tx.AmountLimit.Count; i++)
            {
                amountBytes[i] = AmountLimitToBytes(tx.AmountLimit[i]);
            }
            encoder.Put(amountBytes);

            var signBytes = new byte[tx.Signatures.Count][];
            for (int i = 0; i < tx.Signatures.Count; i++)
            {
                signBytes[i] = SignatureToBytes(tx.Signatures[i]);
            }
            encoder.Put(signBytes);

            return encoder.GetBytes();
        }

        protected Rpcpb.Action NewAction(string contract, string name, string data)
        {
            return new Rpcpb.Action()
            {
                Contract = contract,
                ActionName = name,
                Data = data
            };
        }

        protected byte[] ActionToBytes(Rpcpb.Action action)
        {
            var tx = TransactionRequest;
            var se = new Helpers.SimpleEncoder(65536);
            se.Put(action.Contract)
              .Put(action.ActionName)
              .Put(action.Data);
            return se.GetBytes();
        }

        protected byte[] AmountLimitToBytes(Rpcpb.AmountLimit amountLimit)
        {
            var tx = TransactionRequest;
            var se = new Helpers.SimpleEncoder(65536);
            se.Put(amountLimit.Token)
              .Put(amountLimit.Value);
            return se.GetBytes();
        }

        protected byte[] SignatureToBytes(Rpcpb.Signature signature)
        {
            var tx = TransactionRequest;
            var se = new Helpers.SimpleEncoder(65536);
            se.Put((byte)signature.Algorithm)
              .Put(signature.Signature_.ToByteArray())
              .Put(signature.PublicKey.ToByteArray());
            return se.GetBytes();
        }
    }
}