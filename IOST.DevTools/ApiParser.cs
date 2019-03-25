using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IOSTSdk.DevTools
{
    public struct ApiParameter
    {
        public string Type;
        public string Name;
        public string Description;
    }

    public class ApiParser
    {
        public string Api { get; private set; }
        public string Description { get; private set; }
        public List<ApiParameter> Parameters { get; } = new List<ApiParameter>();

        private readonly HtmlNode _apiNode;
        private readonly string _apiPrefix;
        private HtmlNode _currentNode;

        internal ApiParser(HtmlNode node, string cid)
        {
            _apiNode = node;
            _currentNode = node;
            _apiPrefix = cid.Replace(".iost", "");
            _apiPrefix = Char.ToUpperInvariant(_apiPrefix[0]) + _apiPrefix.Substring(1);

            Api = ExtractApiName(node.InnerText);

            InitDescription();
            InitParameters();
        }

        protected void InitDescription()
        {
            var node = NextDescription();
            Description = node.InnerText;
        }

        protected void InitParameters()
        {
            var node = NextParametersTable();
            var trs = node.SelectNodes("tbody/tr");
            var ths = node.SelectNodes("thead/tr/th");

            Func<HtmlNodeCollection, int, string> fnGetType = null, fnGetName = null, fnGetDesc = (col, _) => "";

            int[] ii = new int[]{ 0, 1, 2 };
            for(int i=0; i<ths.Count; i++)
            {
                var th = ths[i];
                var text = th.InnerText.ToLower();
                var index = ii[i];
                if (text.Contains("remarks") ||
                    text.Contains("description"))
                {
                    fnGetDesc = (col, _) => col[index].InnerText;
                }
                else if (text.Contains("type"))
                {
                    fnGetType = (col, _) => col[index].InnerText;
                }
                else if (text.Contains("name") ||
                         text.Contains("parameter lis"))
                {
                    fnGetName = (col, _) => col[index].InnerText;
                }
                else if (text.Contains("meaning"))
                {
                    fnGetDesc = (col, _) => col[index].InnerText;
                    if (fnGetName == null)
                    {
                        fnGetName = (col, j) => $"arg{j}";
                    }
                }
            }

            foreach (var tr in trs)
            {
                var j = Parameters.Count;
                var tds = tr.SelectNodes("td");
                var param = new ApiParameter
                {
                    Name = EnsureNotEmpty(fnGetName(tds, j).ToCamelcase()),
                    Type = EnsureNotEmpty(fnGetType(tds, j)),
                    Description = fnGetDesc(tds, j)
                };

                Parameters.Add(param);
            }
        }

        protected HtmlNode NextParametersTable()
        {
            var next = _currentNode.NextSiblingIgnoreText();
            while (true)
            {
                if (next == null || next.Name == "h2")
                {
                    throw new InvalidOperationException($"failed to find Description for API: {Api}");
                }
                else if(next.Name == "table")
                {
                    _currentNode = next;
                    break;
                }

                next = next.NextSiblingIgnoreText();
            }

            return next;
        }

        public void WriteBegin(StreamWriter writer)
        {
            var name = Char.ToUpperInvariant(Api[0]) + Api.Substring(1);
            var args = CreateArgs();

            var s = $"\r\n        public static Transaction {_apiPrefix}{name}(this {args})" +
                     "\r\n        {";
            writer.Write(s);
        }

        protected string CreateArgs()
        {
            string args = "Transaction tx";
            foreach (var param in Parameters)
            {
                args += $", {TypeMap(param.Type)} {param.Name}";
            }

            return args;
        }

        public void WriteEnd(StreamWriter writer)
        {
            var s = "\r\n        }";
            writer.Write(s);
        }

        public void WriteBody(StreamWriter writer)
        {
            var parameters = GetCallParameters();
            var s = $"\r\n            tx.AddAction(Cid, \"{Api}\", {parameters});" +
                     "\r\n            return tx;";
            writer.Write(s);
        }

        private string GetCallParameters()
        {
            string parameters = "";
            foreach (var param in Parameters)
            {
                if (parameters.Length > 0)
                {
                    parameters += ", ";
                }
                parameters += $"{param.Name}";
            }

            return parameters;
        }

        public void WriteDocs(StreamWriter writer)
        {
            var desc = Description.ReplaceNewLines("\r\n        /// ");
            var paramDocs = CreateParamDocs();
            var s = @"        /// <summary>" +
                    $"\r\n        /// {desc}" +
                     "\r\n        /// </summary>";
            s += paramDocs;
            writer.Write(s);
        }

        protected string CreateParamDocs()
        {
            string docs = "";
            foreach(var param in Parameters)
            {
                var desc = param.Description
                                .ReplaceNewLines("\r\n        /// ");
                docs += $"\r\n        /// <param name=\"{param.Name}\">{desc}</param>";
            }

            return docs;
        }

        protected HtmlNode NextDescription()
        {
            var next = _currentNode.NextSiblingIgnoreText();
            if (next == null || next.Name == "h2")
            {
                throw new InvalidOperationException($"failed to find Description for API: {Api}");
            }
            else if (next.Name == "p")
            {
                _currentNode = next;
            }

            return next;
        }

        private string EnsureNotEmpty(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new InvalidOperationException("parameter string must not be empty");
            }

            return s;
        }

        private static string TypeMap(string s)
        {
            s = s.ToLowerInvariant();
            if (s.Contains("number"))
            {
                s = "int";
            }
            else if (s.Contains("json"))
            {
                s = "object";
            }
            return s.ToLowerInvariant();
        }

        private static string ExtractApiName(string s)
        {
            var i = s.IndexOf("(");
            if (i < 0)
            {
                return s;
            }
            return s.Substring(0, i).Trim();
        }
    }
}
