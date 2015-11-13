using System.Collections.Generic;
using System.Linq;

namespace MagicDrafter
{
    public class PairingUtility
    {
        public static int GetPlayerPoints(Player piPlayer, List<Match> piMatches)
        {
            int wins = 0;
            int draws = 0;
            var playersMatches = piMatches.Where(p => p.Players.Contains(piPlayer));

            foreach (Match match in playersMatches)
            {
                if (match.IsDraw())
                {
                    draws++;
                    continue;
                }

                Player player = match.GetWinningPlayer();

                if (player == piPlayer)
                    wins++;
            }
            piPlayer.Points = (wins * 3) + (draws * 1);
            return piPlayer.Points;
        }

        public static float GetPlayerOpponentWinPercent(Player piPlayer, List<Match> piMatches)
        {
            var playersMatches = piMatches.Where(p => p.Players.Contains(piPlayer));
            int wins = 0;
            int losses = 0;

            foreach (Match match in playersMatches)
            {
                Player opponent = match.GetOpponentOf(piPlayer);
                var opponentsMatches = piMatches.Where(p => p.Players.Contains(opponent));              

                foreach (Match opponentMatch in opponentsMatches)
                {
                    wins += opponentMatch.GetWinsOfPlayer(opponent);
                    losses += opponentMatch.GetLossesOfPlayer(opponent);
                }
            }

            piPlayer.TieBreaker = (float)wins / (wins + losses);
                
            return (float)wins / (wins + losses);
        }

        public static bool IsMatchValid(List<Match> piMatches, Match piNewMatch)
        {
            return !piMatches.Any(m => m.Players.Contains(piNewMatch.Players[0]) && m.Players.Contains(piNewMatch.Players[1]));
        }

        public static List<Player> GetPlayersOrdered(List<Match> piPreviousMatches, List<Player> piPlayers)
        {
            List<Player> players = piPlayers.OrderByDescending(p => PairingUtility.GetPlayerPoints(p, piPreviousMatches)).ThenByDescending(p => PairingUtility.GetPlayerOpponentWinPercent(p, piPreviousMatches)).ToList();

            return SetByeToLastPlayer(players);
        }

        public static List<Player> GetPlayersSortedSpecifiedFirst(List<Player> piPlayers, Player piFirstPlayer)
        {
            var result = new List<Player>();
            result.Add(piFirstPlayer);

            foreach (Player player in piPlayers)
            {
                if (!result.Contains(player))
                    result.Add(player);
            }

            return SetByeToLastPlayer(result);
        }

        private static List<Player> SetByeToLastPlayer(List<Player> players)
        {
            Player bye = players.FirstOrDefault(p => p.Name == "Bye");

            if (bye == null)
                return players;

            var result = new List<Player>();
            foreach (Player player in players)
            {
                if (player != bye)
                    result.Add(player);
            }

            result.Add(bye);

            return result;
        }
    }
}
