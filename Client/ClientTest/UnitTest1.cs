
using Client;

namespace ClientTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestValidIpAddress()
        {
            var client = new Clients();
            string validIpAddress = "127.0.0.1";

            bool result = client.IsValidIp(validIpAddress);

            Assert.IsTrue(result);
            Assert.AreEqual(string.Empty, client.ErrorMsg);
        }

        [TestMethod]
        public void TestInValidIpAddress()
        {
            var client = new Clients();
            string invalidIpAddress = "999.999.999.999";

            bool result = client.IsValidIp(invalidIpAddress);

            Assert.IsFalse(result);
            Assert.AreEqual("Adresse IP invalide.", client.ErrorMsg);
        }
    }
}