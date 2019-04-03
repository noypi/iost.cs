using IOSTSdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace IOSTSdk.Test
{
    [TestClass]
    public class ApiTests
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public async Task TestGetProducers()
        {
            var client = Client.NewJapan();
            var iost = new IOST(client);

            var producers = await iost.GetProducers256();
            Assert.IsTrue(0 < producers.Count);
            
            foreach (var s in producers)
            {
                Debug.WriteLine(s);
            }
        }
    }
}
