using MagicDrafter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class RoundTests
    {
        [TestMethod]
        public void RoundPair_6players_EasyPairInvalid()
        {
            List<Player> players = Get6PlayerPool();

            List<Match> matches = new List<Match>()
            {
                new Match(players[2], players[5]),
                new Match(players[0], players[3]),
                new Match(players[4], players[1]),
                new Match(players[2], players[0]),
                new Match(players[4], players[3]),
                new Match(players[5], players[1])
            };

            matches[0].Score = new List<int>() { 2, 0 };
            matches[1].Score = new List<int>() { 2, 0 };
            matches[2].Score = new List<int>() { 2, 0 };

            matches[3].Score = new List<int>() { 2, 0 };
            matches[4].Score = new List<int>() { 0, 2 };
            matches[5].Score = new List<int>() { 2, 0 };

            bool r = players[0] == null;

            var round = new Round();

            round.RoundPair(matches, players);

            Assert.IsTrue(round.Matches.Any(m => m.Players.Contains(players[2]) && m.Players.Contains(players[4])));
            Assert.IsTrue(round.Matches.Any(m => m.Players.Contains(players[0]) && m.Players.Contains(players[5])));
            Assert.IsTrue(round.Matches.Any(m => m.Players.Contains(players[1]) && m.Players.Contains(players[3])));
        }

        [TestMethod]
        public void IsReported_NoMatchesReported_False()
        {
            var round = new Round();

            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));
            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));
            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));

            Assert.IsFalse(round.IsRoundReported());
        }

        [TestMethod]
        public void IsReported_OneMatchesReported_False()
        {
            var round = new Round();

            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));
            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));
            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));

            round.Matches[0].Reported = true;

            Assert.IsFalse(round.IsRoundReported());
        }

        [TestMethod]
        public void IsReported_AllMatchesReported_True()
        {
            var round = new Round();

            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));
            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));
            round.Matches.Add(new Match(new Player("asd"), new Player("ds")));

            round.Matches[0].Reported = true;
            round.Matches[1].Reported = true;
            round.Matches[2].Reported = true;

            Assert.IsTrue(round.IsRoundReported());
        }

        private static List<Player> Get8PlayerPool()
        {
            var result = Get6PlayerPool();
            result.Add(new Player("Stoffe"));
            result.Add(new Player("Hitler"));

            return result;
        }

        private static List<Player> Get6PlayerPool()
        {
            return new List<Player>()
            {
                new Player("Philip"),
                new Player("Simon"),
                new Player("Johan"),
                new Player("Ogge"),
                new Player("Sam"),
                new Player("Adam"),
            };
        }
    }
}
