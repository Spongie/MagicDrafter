using MagicDrafter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class MatchTests
    {
        public void GetOpponentOf_GivesOtherPlayer()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);

            Assert.AreEqual(player1, match.GetOpponentOf(player2));
            Assert.AreEqual(player2, match.GetOpponentOf(player1));
        }

        [TestMethod]
        public void GetOpponentOf_InputIsNotInMatch_Null()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");
            var player3 = new Player("dsaasdaadsa");

            var match = new Match(player1, player2);

            Assert.IsNull(match.GetOpponentOf(player3));
        }

        [TestMethod]
        public void RegisterScore_RegisteredIsTrue()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);
            match.RegisterScore(1, 2);

            Assert.IsTrue(match.Reported);
        }

        [TestMethod]
        public void RegisterScore_ScoreUpdated()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);
            match.RegisterScore(1, 2);

            Assert.AreEqual(1, match.Score[0]);
            Assert.AreEqual(2, match.Score[1]);
        }

        [TestMethod]
        public void RegisterScore_GetWinningPlayer()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);
            match.RegisterScore(1, 2);

            Assert.AreEqual(player2, match.GetWinningPlayer());
        }

        [TestMethod]
        public void RegisterScore_WinsOfPlayer()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);
            match.RegisterScore(1, 2);

            Assert.AreEqual(1, match.GetWinsOfPlayer(player1));
            Assert.AreEqual(2, match.GetWinsOfPlayer(player2));
        }

        [TestMethod]
        public void RegisterScore_LossesOfPlayer()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);
            match.RegisterScore(1, 2);

            Assert.AreEqual(2, match.GetLossesOfPlayer(player1));
            Assert.AreEqual(1, match.GetLossesOfPlayer(player2));
        }

        [TestMethod]
        public void IsDraw_EqualScore_true()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);
            match.RegisterScore(1, 1);

            Assert.IsTrue(match.IsDraw());
        }

        [TestMethod]
        public void IsDraw_UnEqualScore_false()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");

            var match = new Match(player1, player2);
            match.RegisterScore(1, 2);

            Assert.IsFalse(match.IsDraw());
        }
    }
}
