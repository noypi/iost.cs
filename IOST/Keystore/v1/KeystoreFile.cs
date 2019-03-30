namespace IOSTSdk.Keystore.v1
{
    using System;
    using System.IO;
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

        internal override void Initialize()
        {
            using (var file = File.Open(_fpath, FileMode.Open))
            using (var rdr = new StreamReader(file))
            {
                var serializer = new JsonSerializer();
                var m = serializer.Deserialize(rdr, typeof(KeystoreModel)) as KeystoreModel;
                if (m.Version != VERSION)
                {
                    throw new ArgumentException($"was expected {VERSION}, current file is {m.Version}.");
                }
                _keys = m;
            }
        }

        public override void Store(KeystoreModel m)
        {
            if (File.Exists(_fpath))
            {
                File.Copy(_fpath, _fpath + ".backup");
            }

            using (var file = File.Open(_fpath, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(file))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, m);
            }
        }
    }
}
