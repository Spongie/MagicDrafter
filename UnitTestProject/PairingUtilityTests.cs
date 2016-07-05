using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MagicDrafterCore;

namespace UnitTestProject
{
    [TestClass]
    public class PairingUtilityTests
    {
        [TestMethod]
        public void GetPlayerPoints_1Win_3Points()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");
            var player3 = new Player("dsa");
            var player4 = new Player("dsaasda");
            var player5 = new Player("dsa");
            var player6 = new Player("dsaasda");

            var matches = new List<Match>();

            var match = new Match(player1, player2);
            match.RegisterScore(2, 0);
            matches.Add(match);

            Assert.AreEqual(3, PairingUtility.GetPlayerPoints(player1, matches));
        }

        [TestMethod]
        public void GetPlayerPoints_2Win_6Points()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");
            var player3 = new Player("dsa");
            var player4 = new Player("dsaasda");
            var player5 = new Player("dsa");
            var player6 = new Player("dsaasda");

            var matches = new List<Match>();

            var match = new Match(player1, player2);
            var match2 = new Match(player1, player3);
            match2.RegisterScore(2, 1);
            match.RegisterScore(2, 0);
            matches.Add(match);
            matches.Add(match2);

            Assert.AreEqual(6, PairingUtility.GetPlayerPoints(player1, matches));
        }

        [TestMethod]
        public void GetPlayerPoints_Draw_1Point()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");
            var player3 = new Player("dsa");
            var player4 = new Player("dsaasda");
            var player5 = new Player("dsa");
            var player6 = new Player("dsaasda");

            var matches = new List<Match>();

            var match = new Match(player1, player2);
            match.RegisterScore(1, 1);
            matches.Add(match);

            Assert.AreEqual(1, PairingUtility.GetPlayerPoints(player1, matches));
        }

        [TestMethod]
        public void GetPlayerPoints_1Win1Draw_4Points()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");
            var player3 = new Player("dsa");
            var player4 = new Player("dsaasda");
            var player5 = new Player("dsa");
            var player6 = new Player("dsaasda");

            var matches = new List<Match>();

            var match = new Match(player1, player2);
            var match2 = new Match(player1, player3);
            match2.RegisterScore(2, 1);
            match.RegisterScore(1, 1);
            matches.Add(match);
            matches.Add(match2);

            Assert.AreEqual(4, PairingUtility.GetPlayerPoints(player1, matches));
        }

        [TestMethod]
        public void GetPlayerPoints_0Win0Draw_0Points()
        {
            var player1 = new Player("dsa");
            var player2 = new Player("dsaasda");
            var player3 = new Player("dsa");
            var player4 = new Player("dsaasda");
            var player5 = new Player("dsa");
            var player6 = new Player("dsaasda");

            var matches = new List<Match>();

            var match = new Match(player1, player2);
            var match2 = new Match(player1, player3);
            match2.RegisterScore(2, 1);
            match.RegisterScore(1, 0);
            matches.Add(match);
            matches.Add(match2);

            Assert.AreEqual(0, PairingUtility.GetPlayerPoints(player2, matches));
        }
    }
}
