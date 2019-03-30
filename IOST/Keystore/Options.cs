namespace IOSTSdk.Keystore
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Options
    {
        private Dictionary<string, string> _opts = new Dictionary<string, string>();

        public Options(string[] args)
        {
            foreach(var opt in args)
            {
                var ss = opt.Split('=');
                if (ss.Length != 2)
                {
                    continue;
                }

                _opts[ss[0].Trim()] = ss[1].Trim();
            }
        }

        public string Get(string k)
        {
            return (!_opts.ContainsKey(k)) ?
                        "" :
                        _opts[k];
        }
    }
}
