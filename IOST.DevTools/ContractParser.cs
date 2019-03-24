using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IOSTSdk.DevTools
{
    public class ContractParser
    {
        public string Cid { get; private set; }
        public string Description { get; private set; }
        public Dictionary<string, string> Infos { get; } = new Dictionary<string, string>();
        public List<ApiParser> Apis { get; } = new List<ApiParser>();

        private readonly HtmlNode _cidNode;
        private HtmlNode _currentNode;
        private readonly string _idPostfix;

        internal ContractParser(HtmlNode cidNode, string idPostfix)
        {
            _cidNode = cidNode;
            _currentNode = cidNode;
            _idPostfix = idPostfix;

            Cid = cidNode.InnerText;

            InitDescription();
            InitInfoTable();
            InitApis();
        }

        protected void InitDescription()
        {
            Debug.WriteLine("+InitDescription");

            //NextDescription();
            var node = NextDescriptionParagraph();
            Description = node.InnerText;

            Debug.WriteLine("-InitDescription");
        }

        protected void InitInfoTable()
        {
            Debug.WriteLine("+InitInfoTable");

            var node = NextInfoTable();
            var trs = node.SelectNodes("tbody/tr");
            foreach (var tr in trs)
            {
                var tds = tr.SelectNodes("td");
                Infos[tds[0].InnerText] = tds[1].InnerText;
            }

            Debug.WriteLine("-InitInfoTable");
        }

        protected void InitApis()
        {
            Debug.WriteLine("+InitApis");

            var node = NextApiHeader();
            while((node = NextApi()) != null)
            {
                Apis.Add(new ApiParser(node));
            }

            Debug.WriteLine("-InitApis");
        }

        protected HtmlNode NextInfoTable()
        {
            var next = _currentNode.NextSiblingIgnoreText();
            while (true)
            {
                if (next.SelectSingleNode($"*[@id='info{_idPostfix}']") != null)
                {
                    next = next.NextSiblingIgnoreText();
                    EnsureNodeName(next, "table");
                    _currentNode = next;
                    break;
                }
                else if (next.Name == "h2" || next == null)
                {
                    throw new InvalidOperationException($"failed to find Info table for cid: {Cid}");
                }
                next = next.NextSiblingIgnoreText();
            }

            return next;
        }

        protected HtmlNode NextApiHeader()
        {
            var next = _currentNode.NextSiblingIgnoreText();
            while (true)
            {
                if (next.SelectSingleNode($"*[@id='api{_idPostfix}']") != null)
                {
                    _currentNode = next;
                    break;
                }
                else if (next.Name == "h2" || next == null)
                {
                    throw new InvalidOperationException($"failed to find API for cid: {Cid}");
                }
                next = next.NextSiblingIgnoreText();
            }

            return next;
        }

        protected HtmlNode NextApi()
        {
            var next = _currentNode.NextSiblingIgnoreText();
            while (next != null)
            {
                if (next.Name == "h4" && next.NextSiblingIgnoreText().Name == "p")
                {
                    _currentNode = next;
                    break;
                }
                else if (next.Name == "h2" || next == null)
                {
                    return null;
                }

                next = next.NextSiblingIgnoreText();
            }

            return next;
        }

        protected HtmlNode NextDescription()
        {
            var next = _currentNode.NextSiblingIgnoreText();
            while (true)
            {
                if (next.SelectSingleNode($"*[@id='description{_idPostfix}']") != null)
                {
                    _currentNode = next;
                    break;
                }
                else if (next.Name == "h2" || next == null)
                {
                    throw new InvalidOperationException($"failed to find Description for cid: {Cid}");
                }

                next = next.NextSiblingIgnoreText();
            }

            return next;
        }

        protected HtmlNode NextDescriptionParagraph()
        {
            var next = _currentNode.NextSiblingIgnoreText();
            while (true)
            {
                if (next.Name == "p")
                {
                    _currentNode = next;
                    break;
                }
                else if (next.Name == "h2" || next == null)
                {
                    throw new InvalidOperationException($"failed to find Description paragraph for cid: {Cid}");
                }

                next = next.NextSiblingIgnoreText();
            }

            return next;
        }

        protected void EnsureNodeName(HtmlNode node, string name)
        {
            if (node.Name != name)
            {
                throw new InvalidOperationException($"Was expecting node name: {name}, got: {node.Name}");
            }
        }

        public string GetClassName()
        {
            var name = Cid.Replace(".iost", string.Empty);
            var ss = name.Replace("_", " ").Split(" ");

            var classname = "";
            foreach (var s in ss)
            {
                var word = s.Trim();
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }

                classname += Char.ToUpperInvariant(word[0]);
                if (word.Length > 1)
                {
                    classname += word.Substring(1);
                }
            }

            return classname;
        }
    }
}
