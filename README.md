# IOST Blockchain SDK for C# 
- SDK to create a wallet, upload a Smart Contract, or use the IOST blockchain for application integration
- Supports .Net Standard 2.0+ / .Net Framework 4.5+ 

## SDK Features
- Uses a Secure Keychain to keep the private key secured
- Implements all APIs
- Implements all queries to IOST contracts: System, Economic, and Token

## Nuget reference
- https://www.nuget.org/packages/IOST

## Using the System Contract

```C#
    using IOSTSdk.Contract.System;
    ...
    var tx = iost.NewTransaction();
    tx.AuthSignUp(...);
```

## Using the Economic Contract

```C#
    using IOSTSdk.Contract.Economic;
    ...
    var tx = iost.NewTransaction();
    tx.RamBuy(...);
```

## Using the Token Contract

```C#
    using IOSTSdk.Contract.Token;
    ...
    var tx = iost.NewTransaction();
    tx.TokenTransfer(...);
```

## Some common transactions in TxBuilder

```C#
    using IOSTSdk;
    ...
    tx.NewAccount(...)
    ...
    tx.Transfer(...)
```

## Buy me a pizza
Thank you, my IOST account is: noypi

## Limitations (for now)
For .Net Framework 4.5+, must define the following functions if Secp256k1 is needed
- IOST.CryptoSignSecp256k1
- IOST.CryptoGetPubkeySecp256k1
- IOST.CryptoGetPubkeySecp256k1Compressed
- IOST.CryptoGeneratePrivateKeySecp256k1

## Setup
### For .Net Framework, add the the following in the csproj file:
```xml
  <ItemGroup>
    <Content Include="..\libsodium.1.0.17\runtimes\win-$(Platform)\native\*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      <Link>libsodium.dll</Link>
    </Content>
  </ItemGroup>
```

## Help
### How to create an account
Go to https://www.iostabc.com/wallet/createaccount 

