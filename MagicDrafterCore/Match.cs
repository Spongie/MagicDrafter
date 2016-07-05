using System.Collections.Generic;
using System.Linq;

namespace MagicDrafterCore
{
    public class Match : BaseObject
    {
        public Match(params Player[] players)
        {
            Score = new List<int>()
            {
                0,
                0
            };

            Players = players.ToList();

            for (int i = 0; i < Players.Count; i++)
            {
                if(players[i].Name == "Bye")
                {
                    int playerIndex = i == 0 ? 1 : 0;
                    Score = new List<int>() { 0, 0 };
                    Score[playerIndex] = 2;
                    Reported = true;
                }
            }
        }

        private List<Player> ivPlayers;

        public List<Player> Players
        {
            get { return ivPlayers; }
            set
            {
                ivPlayers = value;
                FirePropertyChanged();
            }
        }

        private List<int> ivScore;

        public List<int> Score
        {
            get { return ivScore; }
            set
            {
                ivScore = value;
                FirePropertyChanged();
            }
        }

        public Player GetOpponentOf(Player piPlayer)
        {
            foreach (Player player in Players)
            {
                if (player != piPlayer && ivPlayers.Contains(piPlayer))
                    return player;
            }

            return null;
        }

        public void RegisterScore(int player1Score, int player2Score)
        {
            Score[0] = player1Score;
            Score[1] = player2Score;
            FirePropertyChanged("Score");
            Reported = true;
        }

        public bool IsDraw()
        {
            return Score[0] == Score[1];
        }

        public Player GetWinningPlayer()
        {
            int winningIndex = Score.IndexOf(Score.Max());

            return Players[winningIndex];
        }

        public int GetWinsOfPlayer(Player piPlayer)
        {
            int playerIndex = Players.IndexOf(piPlayer);

            return Score[playerIndex];
        }

        public int GetLossesOfPlayer(Player piPlayer)
        {
            int playerIndex = Players.IndexOf(piPlayer);

            if (playerIndex == 0)
                playerIndex = 1;
            else
                playerIndex = 0;

            return Score[playerIndex];
        }

        private bool isReported;

        public bool Reported
        {
            get { return isReported; }
            set
            {
                isReported = value;
                FirePropertyChanged();
            }
        }

        public override string ToString()
        {
            return string.Format("{0, -12} vs {1, 12} | Score: {2} - {3}", Players[0], Players[1], Score[0], Score[1]);
        }
    }
}
