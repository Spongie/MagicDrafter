using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MagicDrafterCore
{
    public class Round : BaseObject
    {
        private int ivRound;
        private ObservableCollection<Match> ivMatches;
        private ObservableCollection<Player> ivPlayers;

        public Round()
        {
            Matches = new ObservableCollection<Match>();
            RoundNr = 0;
        }

        public Round(List<Match> piPreviousMatches, List<Player> piPlayers, int piRoundNr) :this(piPreviousMatches, piPlayers, piRoundNr, true)
        {
            
        }

        public Round(List<Match> piPreviousMatches, List<Player> piPlayers, int piRoundNr, bool piDoPairings)
        {
            Players = new ObservableCollection<Player>();
            Matches = new ObservableCollection<Match>();
            RoundNr = piRoundNr;

            if (piDoPairings)
            {
                if (piRoundNr == 1)
                    RandomPairing(piPlayers);
                else
                    RoundPair(piPreviousMatches, piPlayers);
            }
            else
            {
                Players = new ObservableCollection<Player>(piPlayers);
            }

            FirePropertyChanged("Matches");
        }

        public void RoundPair(List<Match> piPreviousMatches, List<Player> piPlayers, int piOffset = 0)
        {
            List<Player> players;

            if (piOffset == 0)
                players = PairingUtility.GetPlayersOrdered(piPreviousMatches, piPlayers);
            else
                players = PairingUtility.GetPlayersSortedSpecifiedFirst(piPlayers, piPlayers[piOffset]);

            Dictionary<int, List<Player>> playersByPoints = GetPlayersGroupedByPoints(players);

            PairUpLowPlayers(playersByPoints);

            List<Player> playersDownpaired = new List<Player>();

            foreach (var point in playersByPoints.Keys)
            {

                foreach (var player in playersDownpaired)
                {
                    playersByPoints[point].Add(player);
                }

                playersDownpaired.Clear();

                players = PairingUtility.GetPlayersOrdered(piPreviousMatches, playersByPoints[point]);

                InnerPairing(piPreviousMatches, players, playersDownpaired);
            }
        }

        private static void PairUpLowPlayers(Dictionary<int, List<Player>> playersByPoints)
        {
            var playersToPairUp = new List<Player>();

            foreach (var key in playersByPoints.Keys.OrderBy(key => key))
            {
                playersByPoints[key].AddRange(playersToPairUp);
                playersToPairUp.Clear();

                if (playersByPoints[key].Count == 1 && key != playersByPoints.Keys.Max())
                {
                    playersToPairUp.Add(playersByPoints[key].First());
                    playersByPoints[key].Clear();
                }

            }
        }

        private static Dictionary<int, List<Player>> GetPlayersGroupedByPoints(List<Player> players)
        {
            var playersByPoints = new Dictionary<int, List<Player>>();

            foreach (var player in players)
            {
                if (playersByPoints.ContainsKey(player.Points))
                    playersByPoints[player.Points].Add(player);
                else
                {
                    playersByPoints.Add(player.Points, new List<Player>());
                    playersByPoints[player.Points].Add(player);
                }
            }

            return playersByPoints;
        }

        private void InnerPairing(List<Match> piPreviousMatches, List<Player> players, List<Player> playersDownpaired)
        {
            var newMatches = new List<Match>();
            var playersAdded = new List<string>();
            
            if (players.Count >= 2)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (PlayerHasMatch(players[i]))
                        continue;

                    if(i + 1 == players.Count)
                    {
                        playersDownpaired.AddRange(players);
                        break;
                    }

                    bool addMatch = true;
                    int opponentOffset = 1;
                    Match match = new Match(players[i], players[i + opponentOffset]);

                    while (!PairingUtility.IsMatchValid(piPreviousMatches, match) || PlayerHasMatch(players[i + opponentOffset]))
                    {
                        opponentOffset++;
                        if (i + opponentOffset >= players.Count)
                        {
                            foreach (var player in players.Where(playr => !playersAdded.Contains(playr.Id)))
                            {
                                playersDownpaired.Add(player);
                            }
                            if(newMatches.Any())
                            {
                                RedoPairing(newMatches, piPreviousMatches, players, playersDownpaired, i);
                                return;
                            }
                            addMatch = false;
                            break;
                        }
                        match = new Match(players[i], players[i + opponentOffset]);
                    }

                    if (addMatch)
                    {
                        playersAdded.Add(players[i].Id);
                        playersAdded.Add(players[i + opponentOffset].Id);
                        Matches.Add(match);
                        newMatches.Add(match);

                        if (i + 1 == players.Count)
                        {
                            foreach (var player in players.Where(playr => !playersAdded.Contains(playr.Id)))
                            {
                                playersDownpaired.Add(player);
                            }
                        }
                    }
                    else
                        break;
                }
            }
            else
            {
                playersDownpaired.AddRange(players);
            }
        }

        public void AddMatch(Player piPlayer1, Player piPlayer2)
        {
            Matches.Add(new Match(piPlayer1, piPlayer2));
        }

        private void RedoPairing(List<Match> matchesToUndo, List<Match> previousMatches, List<Player> players, List<Player> playersDownpaired, int indexOfPlayerToPairUp)
        {
            playersDownpaired.Clear();
            foreach (var newMatch in matchesToUndo)
            {
                Matches.Remove(newMatch);
            }

            InnerPairing(previousMatches, PairingUtility.GetPlayersSortedSpecifiedFirst(players, players[indexOfPlayerToPairUp]), playersDownpaired);
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

        public ObservableCollection<Player> Players
        {
            get { return ivPlayers; }
            set
            {
                ivPlayers = value;
                FirePropertyChanged();
            }
        }

        private bool ivRoundReported;

        public bool RoundReported
        {
            get { return ivRoundReported; }
            set
            {
                ivRoundReported = value;
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

        public bool IsEmpty()
        {
            return !Matches.Any();
        }
    }
}
