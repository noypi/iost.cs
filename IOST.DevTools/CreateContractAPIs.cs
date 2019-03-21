using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IOST.DevTools
{
    using HtmlAgilityPack;
    using System;
    using System.Diagnostics;
    using System.IO;

    [TestClass]
    public class CreateContractAPIs
    {
        [TestMethod]
        public void CreateSystemContract()
        {
            var url = "https://developers.iost.io/docs/en/6-reference/SystemContract.html";

            var fileCreator = new ContractFileCreator(url, @".\generated\Contract\System\", "System");
            fileCreator.Create();
        }

        [TestMethod]
        public void CreateEconomicContract()
        {
            var url = "https://developers.iost.io/docs/en/6-reference/EconContract.html";

            var fileCreator = new ContractFileCreator(url, @".\generated\Contract\Economic\", "Economic");
            fileCreator.Create();
        }

        [TestMethod]
        public void CreateTokenContract()
        {
            var url = "https://developers.iost.io/docs/en/6-reference/TokenContract.html";

            var fileCreator = new ContractFileCreator(url, @".\generated\Contract\Token\", "Token");
            fileCreator.Create();
        }
    }
}
