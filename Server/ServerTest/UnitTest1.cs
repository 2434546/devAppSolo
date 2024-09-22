using System.Net.Sockets;
using System.Net;
using Server;
using Serveur;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestChoixBateau()
        {
            var server = new Servers();

            server.ChoixBateau("5,6");

            Assert.IsTrue(server.tableauJoueur[4] == "BB");
        }

        [TestMethod]
        public void TestVerifierGagnant()
        {
            var server = new Servers();

            server.ChoixBateau("5,6");

            Assert.IsFalse(server.VerifierGagnant());
        }
    }
}