# IOST Blockchain SDK for C# 
Support .Net Standard 2.0+ / .Net Framework 4.5+ 

## Features
- Uses a Secure Keychain to keep the private key secured

## Limitations
For .Net Framework 4.5+, must define the following functions if Secp256k1 if needed
- IOST.CryptoSignSecp256k1
- IOST.CryptoGetPubkeySecp256k1
- IOST.CryptoGetPubkeySecp256k1Compressed
- IOST.CryptoGeneratePrivateKeySecp256k1

## Help
### How to create an account
Go to https://www.iostabc.com/wallet/createaccount 
