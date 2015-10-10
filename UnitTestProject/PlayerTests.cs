using MagicDrafter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Player_NameAlwaysUpperletter()
        {
            var player = new Player("asdjhasdj");

            Assert.AreEqual("A", player.Name[0].ToString());
        }
    }
}
