namespace IOSTSdk.Keystore.v1
{
    using System;
    using System.IO;
    using System.Linq;
    using IOSTSdk.Crypto;
    using Newtonsoft.Json;

    internal class KeystoreFile : AbstractKeystore
    {
        public static readonly string VERSION = "1.0";

        private readonly string _fpath;

        private KeystoreModel _keys = null;

        /// <summary>
        /// Example args:
        ///     "file=./path/to/keystore.txt"
        /// </summary>
        /// <param name="args"></param>
        internal KeystoreFile(Options opts)
        {
            _fpath = opts.Get("file");
            if (string.IsNullOrEmpty(_fpath))
            {
                throw new ArgumentException($"invalid file path: {_fpath}");
            }
        }

        public override EncryptedKey[] EncryptedKeys => _keys.Keys;

        public override void Initialize()
        {
            using (var file = File.Open(_fpath, FileMode.Open))
            using (var rdr = new StreamReader(file))
            {
                var serializer = new JsonSerializer();
                var m = serializer.Deserialize(rdr, typeof(KeystoreModel)) as KeystoreModel
                            ?? new KeystoreModel()
                               {
                                   Version = "1.0"
                               };
                if (m.Version != VERSION)
                {
                    throw new ArgumentException($"was expected {VERSION}, current file is {m.Version}.");
                }
                _keys = m;
            }
        }

        public override void Store()
        {
            Backup();

            using (var file = File.Open(_fpath, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(file))
            {
                var json = JsonConvert.SerializeObject(_keys, Formatting.Indented);
                writer.WriteLine(json);
            }
        }

        public override void ChangePassword(SecureBytes oldPassword, SecureBytes newPassword)
        {
            var newKeys = new KeystoreModel()
            {
                Version = _keys.Version
            };

            var oldKeys = _keys.Keys;
            _keys = newKeys;

            foreach(var k in oldKeys)
            {
                AddKey(k.Decrypt(oldPassword), newPassword, k.Label);
            }

            Store();
        }

        public override EncryptedKey ChangeEncryptedKeyPassword(EncryptedKey enckey, SecureBytes oldPassword, SecureBytes newPassword)
        {
            byte[] cipher = null;
            byte[] nonce = null;
            enckey.Decrypt(oldPassword).UseUnprotected(bb => (cipher, nonce) = SodiumXChaCha20Poly1305.Encrypt(newPassword, bb));

            return new EncryptedKey
            {
                Label = enckey.Label,
                Cipher = $"{Convert.ToBase64String(cipher)}",
                Nonce = Convert.ToBase64String(nonce)
            };
        }

        public override void AddKey(SecureBytes privateKey, SecureBytes password, string label)
        {
            byte[] cipher = null;
            byte[] nonce = null;
            privateKey.UseUnprotected(bb => (cipher, nonce) = SodiumXChaCha20Poly1305.Encrypt(password, bb));
            var enckey = new EncryptedKey()
            {
                Label = label,
                Cipher = $"{Convert.ToBase64String(cipher)}",
                Nonce = Convert.ToBase64String(nonce)
            };

            var newkeys = new EncryptedKey[_keys.Keys.Length + 1];
            Array.Copy(_keys.Keys, newkeys, _keys.Keys.Length);
            newkeys[_keys.Keys.Length] = enckey;
            _keys.Keys = newkeys;
        }

        public override void RenameKeyLabel(EncryptedKey enckey, string newLabel)
        {
            var index = GetKeyIndex(enckey.Label, enckey);

            if (index >= 0)
            {
                _keys.Keys[index].Label = newLabel;
            }
        }

        public override void DeleteKey(string label, EncryptedKey enckey)
        {
            _keys.Keys = _keys.Keys
                                .Where(x => (x.Nonce != enckey.Nonce) || (x.Label != enckey.Label))
                                .Select(x => x)
                                .ToArray();
        }

        protected void Backup()
        {
            if (File.Exists(_fpath))
            {
                File.Copy(_fpath, _fpath + ".backup");
            }
        }

        protected int GetKeyIndex(string label, EncryptedKey enckey)
        {
            int index = -1;
            for (int i = 0; i < _keys.Keys.Length; i++)
            {
                var k = _keys.Keys[i];
                if (enckey.Nonce == k.Nonce && label == enckey.Label)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
    }
}
