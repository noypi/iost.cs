namespace IOSTSdk.Crypto
{
    using System;
    using IOSTSdk.Helpers;
    
    /// <summary>
    /// https://download.libsodium.org/doc/public-key_cryptography
    /// </summary>
    public static class SodiumEd25519
    {
        /// <summary>
        /// https://github.com/jedisct1/libsodium/blob/6934a8d0c8eea550e5ab664beff0352e4b569efd/src/libsodium/include/sodium/crypto_sign_ed25519.h
        /// </summary>
        public static readonly uint SEEDBYTES = 32;
        public static readonly uint PUBLICKEYBYTES = 32;
        public static readonly uint SECRETKEYBYTES = 64;

        // crypto_sign_ed25519_BYTES 
        public static readonly uint SIGNBYTES = 64;

        private static readonly IAlgorithm _Ed25519 = new Crypto.Ed25519();
        
        public static byte[] RandomBytes(int n)
        {
            var buf = new byte[n];
            Sodium.randombytes_buf(buf, n);
            return buf;
        }
        
        public static KeyPair NewKeyPair(byte[] seed)
        {
            var publicKey = new byte[PUBLICKEYBYTES];
            var privateKey = new byte[SECRETKEYBYTES];

            Sodium.crypto_sign_seed_keypair(publicKey, privateKey, seed);

            var kp = new KeyPair(new SecureBytes(privateKey), _Ed25519);
            DataHelper.DestroyData(privateKey);

            return kp;
        }

        public static KeyPair NewKeyPair()
        {
            var seed = RandomBytes((int)SEEDBYTES);
            return NewKeyPair(seed);
        }

        public static byte[] ExtractPublicKey(SecureBytes seckey)
        {
            byte[] pubkey = null; 
            seckey.UseUnprotected(privkey =>
            {
                if (privkey?.Length != SECRETKEYBYTES)
                {
                    throw new ArgumentOutOfRangeException("invalid secret key");
                }

                pubkey = new byte[PUBLICKEYBYTES];
                if (0 != Sodium.crypto_sign_ed25519_sk_to_pk(pubkey, privkey))
                {
                    throw new InvalidOperationException("failed to extract public key");
                }
            });

            return pubkey;
        }

        public static bool VerifyDetached(byte[] signature, byte[] message, byte[] pubkey)
        {
            if ((signature?.Length != SIGNBYTES) ||
                (pubkey?.Length != PUBLICKEYBYTES))
            {
                throw new ArgumentOutOfRangeException("invalid parameter");
            }

            return (0 == Sodium.crypto_sign_verify_detached(signature, message, message.Length, pubkey));
        }

        public static byte[] SignDetached(byte[] message, SecureBytes seckey)
        {
            var signed = new byte[SIGNBYTES];

            seckey.UseUnprotected(privkey =>
            {
                if(privkey?.Length != SECRETKEYBYTES)
                {
                    throw new ArgumentOutOfRangeException("invalid secret key");
                }

                long n = 0;
                Sodium.crypto_sign_detached(signed, ref n, message, message.Length, privkey);
            });

            return signed;
        }
    }
}
