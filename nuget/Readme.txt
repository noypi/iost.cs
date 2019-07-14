IOST-1.1.6
- add IKeystore.RenameKeyLabel()
- add ChangeEncryptedKeyPassword();
- update dependency versions

IOST-1.1.5
- Fixes in Rpcpb namespace.
- Expose Transaction.TxRequest

IOST-1.1.4
- Remove ChainPay from Nuget dependencies

IOST-1.1.3
- Decouple the experimental ChainPay library
- IOST.Send to return the TxHash if TxReceipt is not successful, instead of throwing an Exception.

IOST-1.1.2
- Add ChangePassword() to IKeystore

IOST-1.1.1
- Expose Initialize() from IKeystore

IOST-1.1.0
- Read / write a keystore file
- Generate a signature request
- Rename contract APIs remove "_" underscore, example Vote_producerXXX to VoteProducer
- Implement SecurePassword
- CreateAccount will have 1 IOST as default gas pledge
- Fix null exception during Send() when receipt is null
- Implement missing APIs

IOST-1.0.0
- Implement all APIs
- Implement all contracts: System, Economic, Token
- Use grpc+protobuff
- Add TxBuilder helper
- Add server helpers, example Client.NewKorea() 
- Support .NetStandard 2.0
- Support .Net45 (see github.com/noypi/iost.cs for limitations)