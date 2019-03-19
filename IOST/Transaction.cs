namespace IOST
{
    using Newtonsoft.Json;
    using System;

    public class Transaction
    {
        public Options Options { get; internal set; }
        public Rpcpb.TransactionRequest TransactionRequest { get; internal set; }

        public Transaction(Options options)
        {
            Options = options;
            TransactionRequest = new Rpcpb.TransactionRequest()
            {
                Time = Helpers.UnixNano(),
                GasLimit = options.GasLimit,
                GasRatio = options.GasRatio,
                Expiration = options.ExpirationInMillis,
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

        protected Rpcpb.Action NewAction(string contract, string name, string data)
        {
            return new Rpcpb.Action()
            {
                Contract = contract,
                ActionName = name,
                Data = data
            };
        }
    }
}