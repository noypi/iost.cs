using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSTSdk.DevTools
{
    public static class HelperExt
    {
        public static HtmlNode NextSiblingIgnoreText(this HtmlNode node)
        {
            var next = node.NextSibling;
            while(true)
            {
                if (next == null)
                {
                    break;
                }
                else if (next.Name == "#text")
                {
                    next = next.NextSibling;
                    continue;
                }

                break;
            }

            return next;
        }

        public static string ReplaceNewLines(this string s, string replace)
        {
            return s.Replace("\n", replace);
        }

        public static string ToCamelcase(this string s)
        {
            var ss = s.Replace("_", " ").Split(" ");
            string result = "";
            foreach (var w in ss)
            {
                var word = w.Trim();
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }

                result += Char.ToUpperInvariant(word[0]);
                if (word.Length > 1)
                {
                    result += word.Substring(1);
                }
            }

            return Char.ToLowerInvariant(result[0]) + result.Substring(1);
        }
    }
}
