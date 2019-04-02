namespace IOSTSdk.Keystore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    public enum KeystoreType
    {
        FromFile,
        FromNative
    }

    public class KeystoreRegistry
    {
        public static readonly string KEYSTOREFILE_KEY_PREFIX = "keystore file v";
        public static readonly string KEYSTORENATIVE_KEY = "keystore native";

        private static readonly string DEFAULT_KEYSTOREFILE_KEY = "DEFAULT_KEYSTORE_FILE";
            
        private static Dictionary<string, Func<Options, IKeystore>> _byName = new Dictionary<string, Func<Options, IKeystore>>();

        static KeystoreRegistry()
        {
            Register($"{KEYSTOREFILE_KEY_PREFIX}{v1.KeystoreFile.VERSION}", opts => new v1.KeystoreFile(opts));
            Register(DEFAULT_KEYSTOREFILE_KEY, opts => new v1.KeystoreFile(opts));

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Register(KEYSTORENATIVE_KEY, opts => new v1.KeystoreWin10Certs(opts));
            }
        }

        public static void Register(string name, Func<Options, IKeystore> constructor)
        {
            _byName[name] = constructor;
        }

        public static IKeystore Create(KeystoreType t, params string[] args)
        {
            var opts = new Options(args);

            AbstractKeystore ks = null;
            switch (t)
            {
                case KeystoreType.FromFile:
                    ks = CreateFromFile(opts);
                    break;
                case KeystoreType.FromNative:
                    ks = CreateFromNative(opts);
                    break;
                
            }

            if (ks == null)
            {
                throw new ArgumentException($"unsupported KeystoreType: {t}, args: { string.Join(", ", args) }");
            }

            ks.Initialize();
            return ks;
        }

        protected static AbstractKeystore CreateFromFile(Options opts)
        {
            var fpath = opts.Get("file");
            if (!File.Exists(fpath))
            {
                return New(DEFAULT_KEYSTOREFILE_KEY, opts);
            }

            string version = string.Empty;

            using (var f = File.Open(fpath, FileMode.Open))
            using (var rdr = new StreamReader(f))
            {
                var serializer = new JsonSerializer();
                var kf = serializer.Deserialize(rdr, typeof(KeystoreFile)) as KeystoreFile;
                version = kf?.Version ?? "1.0";
            }

            var ks = New($"{KEYSTOREFILE_KEY_PREFIX}{version}", opts);
            if (ks == null)
            {
                throw new InvalidOperationException($"no KeystoreFile can handle {KEYSTOREFILE_KEY_PREFIX}{version}, version: {version}");
            }

            return ks;
        }

        protected static AbstractKeystore CreateFromNative(Options opts)
        {
            return New(KEYSTORENATIVE_KEY, opts);
        }

        protected static AbstractKeystore New(string name, Options opts)
        {
            var obj = (_byName.ContainsKey(name)) ?
                                _byName[name](opts) :
                                null;
            return (AbstractKeystore)obj;
        }
    }
}
