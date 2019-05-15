using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSTSdk.Test
{
    [TestClass]
    public class SignatureRequestTests
    {
        [TestInitialize]
        public void Initialize()
        {

        }
#if false
        [TestMethod]
        public void TestReadGenerateSR()
        {
            var iost = new IOST(null);

            var tx = iost.NewTransaction();

            var pubk = "Gcv8c2tH8qZrUYnKdEEdTtASsxivic2834MQW6mgxqto";
            tx.CreateAccount("newname", "creator", pubk, pubk);

            var bb1 = tx.BytesForSigning();

            string sig = tx.CreateSignatureRequest("some tag", Transaction.SRTRANSFERMETHOD_CLIENTCANTRANFER);

            var readSig = Transaction.ReadSignatureRequest(sig);

            CollectionAssert.AreEqual(bb1, Transaction.ToBytesForSigning(readSig.TransferDetails));
            
        }
#endif
    }
}
