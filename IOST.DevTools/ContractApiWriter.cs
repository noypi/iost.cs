using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IOSTSdk.DevTools
{
    public class ContractApiWriter
    {
        private StreamWriter _writer;
        private ContractParser _parser;
        private readonly string _package;
        private readonly string _className;

        public ContractApiWriter(string package, ContractParser parser, StreamWriter writer)
        {
            _parser = parser;
            _writer = writer;
            _package = package;
            _className = _parser.GetClassName();
        }

        public void WriteBegin()
        {
            var s = $"// generated by IOST.DevTools last: {DateTime.Now}" +
                    $"\r\nnamespace {_package}" +
                     "\r\n{" +
                     "\r\n     using IOSTSdk;" +
                     "\r\n";
            _writer.Write(s);
        }

        public void WriteContractDescription(string srcUrl)
        {
            var desc = _parser.Description
                              .ReplaceNewLines("\r\n    /// ");
            var s =  "\r\n    /// <summary>" +
                    $"\r\n    /// {desc}" +
                     "\r\n    /// " +
                    $"\r\n    /// Version: {_parser.Infos["language"]}" +
                    $"\r\n    /// Language: {_parser.Infos["version"]}" +
                    $"\r\n    /// Reference: {srcUrl}" +
                     "\r\n    /// </summary>";
            _writer.Write(s);
        }

        public void WriteBeginClass()
        {
            var s = $"\r\n    public static class {_className}" +
                     "\r\n    {";
            _writer.Write(s);
        }

        public void WriteEndClass()
        {
            var s = "\r\n    }";
            _writer.Write(s);
        }

        public void WriteDeclareVariables()
        {
            var s = $"\r\n        public const string Cid = \"{_parser.Cid}\";";
            _writer.Write(s);
        }

        public void WriteApis()
        {
            foreach(var api in _parser.Apis)
            {
                _writer.Write("\r\n");
                _writer.Write("\r\n");
                api.WriteDocs(_writer);
                api.WriteBegin(_writer);
                api.WriteBody(_writer);
                api.WriteEnd(_writer);
            }
        }

        public void WriteEnd()
        {
            var s = "\r\n}\r\n";
            _writer.Write(s);
        }
    }
}
