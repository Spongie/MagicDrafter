using System.Collections.Generic;
using System.Linq;

namespace MagicDrafter
{
    public class PairingUtility
    {
        public static void SetPlayerTieBreakers(Player player, List<Match> matches)
        {
            player.Points = GetPlayerPoints(player, matches);
            player.OpponentWinPercent = GetPlayerOpponentWinPercent(player, matches);
            player.GameWinPercent = GetPlayerGameWinPercent(player, matches);
            player.OpponentGameWinPercent = GetOpponentsGameWinPercent(player, matches);
        }

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
            
            return (wins * 3) + (draws * 1);
        }

        private static float GetOpponentsGameWinPercent(Player player, List<Match> matches)
        {
            var playersMatches = matches.Where(p => p.Players.Contains(player));
            float wins = 0;
            float losses = 0;

            foreach (Match match in playersMatches)
            {
                Player opponent = match.GetOpponentOf(player);
                var opponentsMatches = matches.Where(p => p.Players.Contains(opponent));

                foreach (Match opponentMatch in opponentsMatches)
                {
                    if (opponentMatch.GetWinningPlayer() == opponent)
                        wins++;
                    else
                        losses++;
                }
            }

            return wins / (wins + losses);
        }

        private static float GetPlayerGameWinPercent(Player player, List<Match> matches)
        {
            float wins = 0;
            float losses = 0;
            var playersMatches = matches.Where(p => p.Players.Contains(player));

            foreach (Match match in playersMatches)
            {
                wins += match.GetWinsOfPlayer(player);
                losses += match.GetLossesOfPlayer(player);
            }

            return wins / (wins + losses);
        }

        public static float GetPlayerOpponentWinPercent(Player piPlayer, List<Match> piMatches)
        {
            var playersMatches = piMatches.Where(p => p.Players.Contains(piPlayer));
            float wins = 0;
            float losses = 0;

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
                
            return wins / (wins + losses);
        }

        public static bool IsMatchValid(List<Match> piMatches, Match piNewMatch)
        {
            return !piMatches.Any(m => m.Players.Contains(piNewMatch.Players[0]) && m.Players.Contains(piNewMatch.Players[1]));
        }

        public static List<Player> GetPlayersOrdered(List<Match> piPreviousMatches, List<Player> piPlayers)
        {
            foreach (Player player in piPlayers)
            {
                SetPlayerTieBreakers(player, piPreviousMatches);
            }

            List<Player> players = piPlayers.OrderByDescending(player => player.Points)
                .ThenByDescending(player => player.OpponentWinPercent).ThenBy(player => player.GameWinPercent).ThenBy(player => player.OpponentGameWinPercent).ToList();

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
