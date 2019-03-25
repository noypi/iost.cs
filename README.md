# IOST Blockchain SDK for C# 
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/355b46fe6614483d9a91459ba173d1bf)](https://app.codacy.com/app/noypi/iost.cs?utm_source=github.com&utm_medium=referral&utm_content=noypi/iost.cs&utm_campaign=Badge_Grade_Dashboard)
[![Nuget](https://img.shields.io/nuget/v/IOST.svg)](http://www.nuget.org/packages/IOST/)
[![Build Status](https://dev.azure.com/adrianmigraso0686/iost.cs/_apis/build/status/noypi.iost.cs?branchName=master)](https://dev.azure.com/adrianmigraso0686/iost.cs/_build/latest?definitionId=1&branchName=master)
- SDK to create a wallet, upload a Smart Contract, or use the IOST blockchain for application integration
- Supports .Net Standard 2.0+ / .Net Framework 4.5+ 

## SDK Features
- Uses a Secure Keychain to keep the private key secured
- Implements all APIs
- Implements all queries to IOST contracts: System, Economic, and Token

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

