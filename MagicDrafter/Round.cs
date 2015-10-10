using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MagicDrafter
{
    public class Round : BaseObject
    {
        private int ivRound;
        private ObservableCollection<Match> ivMatches;

        public Round()
        {
            Matches = new ObservableCollection<Match>();
            RoundNr = 0;
        }

        public Round(List<Match> piPreviousMatches, List<Player> piPlayers, int piRoundNr)
        {
            Matches = new ObservableCollection<Match>();
            RoundNr = piRoundNr;

            if (piRoundNr == 1)
                RandomPairing(piPlayers);
            else
                RoundPair(piPreviousMatches, piPlayers);

            FirePropertyChanged("Matches");
        }

        public void RoundPair(List<Match> piPreviousMatches, List<Player> piPlayers, int piOffset = 0)
        {
            List<Player> players;
            bool broken = false;

            if (piOffset == 0)
                players = PairingUtility.GetPlayersOrdered(piPreviousMatches, piPlayers);
            else
                players = PairingUtility.GetPlayersSortedSpecifiedFirst(piPlayers, piPlayers[piOffset]);

            for (int i = 0; i + 1 < players.Count; i ++)
            {
                if (PlayerHasMatch(players[i]))
                    continue;

                int opponentOffset = 1;
                Match match = new Match(players[i], players[i + opponentOffset]);

                while(!PairingUtility.IsMatchValid(piPreviousMatches, match) || PlayerHasMatch(players[i + opponentOffset]))
                {
                    opponentOffset++;
                    if(i + opponentOffset >= players.Count)
                    {
                        Matches.Clear();
                        RoundPair(piPreviousMatches, players, i);
                        broken = true;
                        break;
                    }
                    match = new Match(players[i], players[i + opponentOffset]);
                }

                if (broken)
                    break;

                Matches.Add(match);
            }
        }

        private bool PlayerHasMatch(Player piPlayer)
        {
            return Matches.Any(match => match.Players.Contains(piPlayer));
        }

        private void RandomPairing(List<Player> piPlayers)
        {
            var rand = new Random();
            var playersRandomOrder = piPlayers.OrderBy(a => rand.Next(int.MaxValue)).ToList();

            for (int i = 0; i + 1 < playersRandomOrder.Count(); i += 2)
            {
                var match = new Match(playersRandomOrder[i], playersRandomOrder[i + 1]);
                Matches.Add(match);
            }
        }

        public int RoundNr
        {
            get { return ivRound; }
            set
            {
                ivRound = value;
                FirePropertyChanged();
            }
        }

        public ObservableCollection<Match> Matches
        {
            get { return ivMatches; }
            set
            {
                ivMatches = value;
                FirePropertyChanged();
            }
        }

        public bool IsRoundReported()
        {
            foreach (Match match in Matches)
            {
                if (!match.Reported)
                    return false;
            }

            return true;
        }
    }
}
