using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IOSTSdk.DevTools
{
    public class ContractFileCreator
    {
        private readonly string _dir;
        private readonly string _srcUrl;
        private readonly string _packageName;

        public ContractFileCreator(string srcUrl, string dir, string packageName)
        {
            _dir = dir;
            _srcUrl = srcUrl;
            _packageName = packageName;
            EnsureDirs(dir);
        }

        public void Create()
        {
            var web = new HtmlWeb();
            var doc = web.Load(_srcUrl);

            var cidH2 = doc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[2]/div/div[1]/article/div/span/h2");

            int i = 0;
            foreach (var node in cidH2)
            {
                var cid = node.InnerText;
                EnsureCid(cid);

                string idPostfix = "";
                if (i > 0)
                {
                    idPostfix = $"-{i}";
                }

                var parser = new ContractParser(node, idPostfix);
                var className = parser.GetClassName();
                var fpath = Path.Combine(_dir, $"{className}.cs");
                fpath = Path.GetFullPath(fpath);
           
                Debug.WriteLine($"fpath: {fpath}");

                using (var writer = new StreamWriter(File.Open(fpath, FileMode.Create)))
                {
                    var fullpackage = $"IOST.Contract.{_packageName}";
                    var contractWriter = new ContractApiWriter(fullpackage, parser, writer);

                    contractWriter.WriteBegin();
                    contractWriter.WriteContractDescription(_srcUrl);
                    contractWriter.WriteBeginClass();
                    contractWriter.WriteDeclareVariables();
                    contractWriter.WriteApis();
                    contractWriter.WriteEndClass();
                    contractWriter.WriteEnd();
                }

                i++;
            }
        }

        private void EnsureDirs(string dirs)
        {
            Directory.CreateDirectory(dirs);
        }

        static void EnsureCid(string cid)
        {
            if (!cid.EndsWith(".iost"))
            {
                throw new InvalidOperationException($"invalid contract id {cid}.");
            }
        }
    }
}
