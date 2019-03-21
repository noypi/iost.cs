using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace IOST.Test
{
    [TestClass]
    public class ApiTests
    {
        //private string _TestServerUrl = "localhost:30002";
        private readonly string _TestServerUrl = "192.168.254.99:30002";

        [TestMethod]
        public async Task TestGetNodeInfo()
        {
            var iost = new Client(_TestServerUrl);
            var nodeInfoResponse = await iost.GetNodeInfo();
            Assert.IsNotNull(nodeInfoResponse);
        }
    }
}
