## IOST Blockchain SDK for C# 
[![CodeFactor](https://www.codefactor.io/repository/github/noypi/iost.cs/badge)](https://www.codefactor.io/repository/github/noypi/iost.cs)
[![Nuget](https://img.shields.io/nuget/v/IOST.svg)](http://www.nuget.org/packages/IOST/)
[![Build Status](https://dev.azure.com/adrianmigraso0686/iost.cs/_apis/build/status/noypi.iost.cs?branchName=master)](https://dev.azure.com/adrianmigraso0686/iost.cs/_build/latest?definitionId=1&branchName=master)
- SDK to create a wallet, upload a Smart Contract, or use the IOST blockchain for application integration
- Supports .Net Standard 2.0+ / .Net Framework 4.5+ 

### SDK Features
- Uses a Secure Keychain to keep the private key secured
- Implements all APIs
- Implements all queries to IOST contracts: System, Economic, and Token

### Using the IOST Contracts

#### System Contract
```C#
    using IOSTSdk.Contract.System;
    ...
    var tx = iost.NewTransaction()
                 .AuthSignUp(...);
```

#### Economic Contract

```C#
    using IOSTSdk.Contract.Economic;
    ...
    var tx = iost.NewTransaction()
                 .RamBuy(...);
```

#### Token Contract

```C#
    using IOSTSdk.Contract.Token;
    ...
    var tx = iost.NewTransaction()
                 .TokenTransfer(...);
```

### Examples

#### Creating a new account

```C#
    var client = Client.NewKorea();
    var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

    var tx = iost.NewTransaction()
                 .CreateAccount(newAccountName, creatorsName, newAccountPublicKey, newAccountPublicKey);
    
    var kc = new Keychain(creatorsName);
    
    kc.AddKey(
        new SecureBytes(IOST.Base58Decode(
            creatorsPrivateKeyInBase58)),
            "active");
    tx.AddApprove("*", "unlimited");
    kc.Sign(tx);
    var hash = await iost.Send(tx);
```

#### Transfer IOST
```C#
    var client = Client.NewJapan();
    var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

    var tx = iost.NewTransaction()
             .Transfer("iost", fromAccountName, toAccountName, amount, "");

    var kc = new Keychain(fromAccountName);
    kc.AddKey(
        new SecureBytes(IOST.Base58Decode(
            fromAccountsPrivateKeyBase58)),
            "active");

    kc.Sign(tx);
    var hash = await iost.Send(tx);
```

#### Vote
```C#
    var client = Client.NewKorea();
    var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

    var tx = iost.NewTransaction()
                 .VoteProducerVote(accountName, candidateAccountName, "10000");

    var kc = new Keychain(accountName);
    kc.AddKey(
        new SecureBytes(IOST.Base58Decode(
            accountPrivateKey)),
            "active");
    tx.AddApprove("*", "unlimited");
    kc.Sign(tx);
    var hash = await iost.Send(tx);
```

#### Unvote
```C#
    var client = Client.NewJapan();
    var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

    var tx = iost.NewTransaction()
                 .VoteProducerUnvote(accountName, candidateAccountName, "10000");

    var kc = new Keychain(accountName);
    kc.AddKey(
        new SecureBytes(IOST.Base58Decode(
            accountPrivateKey)),
            "active");
    tx.AddApprove("*", "unlimited");
    kc.Sign(tx);
    var hash = await iost.Send(tx);
```

#### Publish a new contract
```C#
    var tx = iost.NewTransaction();

    using (var abiFile = File.OpenRead(Path.Combine(WORKDIR, "abi.json")))
    using (var abiRdr = new StreamReader(abiFile))
    using (var jsFile = File.OpenRead(Path.Combine(WORKDIR, "code.js")))
    using (var jsRdr = new StreamReader(jsFile))
    {
        tx.PublishContract(abiRdr, jsRdr);
    }

    var kc = new Keychain(accountName);
    kc.AddKey(
        new SecureBytes(IOST.Base58Decode(
            accountPrivateKey)),
            "active");
    tx.AddApprove("*", "unlimited");
    kc.Sign(tx);
    return iost.Send(tx);
```

#### Update contract
```C#
    var tx = _iost.NewTransaction();

    using (var abiFile = File.OpenRead(Path.Combine(WORKDIR, "abi2.json")))
    using (var abiRdr = new StreamReader(abiFile))
    using (var jsFile = File.OpenRead(Path.Combine(WORKDIR, "code.js")))
    using (var jsRdr = new StreamReader(jsFile))
    {
        tx.UpdateContract("ContractF5qwDudF1z5RFnUbJ9muguX4CF1ptex8nBbgZWmRuZ9b", abiRdr, jsRdr, "");
    }

    var kc = new Keychain(accountName);
    kc.AddKey(
        new SecureBytes(IOST.Base58Decode(
            accountPrivateKey)),
            "active");
    kc.Sign(tx);
    var hash = await _iost.Send(tx);
```

### Buy me a pizza
Thank you, my IOST account is: noypi

### Limitations (for now)
For .Net Framework 4.5+, must define the following functions if Secp256k1 is needed
- IOST.CryptoSignSecp256k1
- IOST.CryptoGetPubkeySecp256k1
- IOST.CryptoGetPubkeySecp256k1Compressed
- IOST.CryptoGeneratePrivateKeySecp256k1

### Setup
#### For .Net Framework, add the the following in the csproj file:
```xml
  <ItemGroup>
    <Content Include="..\packages\libsodium.1.0.17\runtimes\win-$(Platform)\native\libsodium.dll">
      <Link>libsodium.dll</Link>
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
```

### Help
#### How to create an account
Go to https://www.iostabc.com/wallet/createaccount 

