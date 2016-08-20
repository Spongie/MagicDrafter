using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MagicDrafterCore
{
    public class Draft : BaseObject
    {
        private ObservableCollection<Player> ivPlayers;
        private List<Match> ivAllMatches;
        public event EventHandler OnNewRoundStart;
        public event EventHandler OnRoundFinished;
        private int ivNumberOfRounds;
        
        public Draft()
        {
            ivPlayers = new ObservableCollection<Player>();
            ivNumberOfRounds = 3;
            ivRounds = new ObservableCollection<Round>();
            ivAllMatches = new List<Match>();

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

        public int NumberOfRounds
        {
            get { return ivNumberOfRounds; }
            set
            {
                ivNumberOfRounds = value;
                FirePropertyChanged();
            }
        }


        private ObservableCollection<Round> ivRounds;

        public ObservableCollection<Round> Rounds
        {
            get { return ivRounds; }
            set
            {
                ivRounds = value;
                FirePropertyChanged();
            }
        }

        private Match ivSelectedMatch;

        public Match SelectedMatch
        {
            get { return ivSelectedMatch; }
            set
            {
                ivSelectedMatch = value;
                FirePropertyChanged();
            }
        }

        public List<Match> Matches
        {
            get { return ivAllMatches; }
            set
            {
                ivAllMatches = value;
                FirePropertyChanged();
            }
        }

        public bool Done { get; set; }
        
        public IEnumerable<Player> GetTemporaryStandings()
        {
            var matches = new List<Match>();
            matches.AddRange(ivAllMatches);
            matches.AddRange(ivRounds.Last().Matches);

            var players = PairingUtility.GetPlayersOrdered(matches, ivPlayers.ToList());

            for (int i = 0; i < players.Count; i++)
            {
                players[i].Rank = i + 1;
            }

            return players;
        }

        public IEnumerable<Player> GetFinalStandings()
        {
            var players = PairingUtility.GetPlayersOrdered(ivAllMatches, ivPlayers.ToList());

            for (int i = 0; i < players.Count; i++)
            {
                players[i].Rank = i + 1;
            }

            return players;
        }

        public bool Start(bool piManualPairing)
        {
            if (ivPlayers.Count % 2 != 0)
                ivPlayers.Add(new Player("Bye"));

            StartNextRound(piManualPairing);
            return true;
        }

        public void StartNextRound(bool piManual, bool sendRoundFinished = true)
        {
            if(ivRounds.Any())
            {
                ivAllMatches.AddRange(ivRounds.Last().Matches);

                if (ivRounds.Last().IsEmpty())
                    ivRounds.RemoveAt(ivRounds.Count - 1);
            }

            if (sendRoundFinished)
                OnRoundFinished?.Invoke(this, new EventArgs());

            ivRounds.Add(new Round(ivAllMatches, ivPlayers.ToList(), ivRounds.Count + 1, !piManual));

            if (!piManual)
                OnNewRoundStart?.Invoke(this, new EventArgs());
        }

        public void FinishDraft()
        {
            Done = true;
            ivAllMatches.AddRange(ivRounds.Last().Matches);
            OnRoundFinished?.Invoke(this, new EventArgs());
        }

        public void RegisterScore(int player1Score, int player2Score)
        {
            SelectedMatch.RegisterScore(player1Score, player2Score);
            FirePropertyChanged("SelectedMatch");
            FirePropertyChanged("Rounds");
        }

        public void ConfirmManualPairing()
        {
            OnNewRoundStart?.Invoke(this, new EventArgs());
        }

        public void CancelManualPairing()
        {
            foreach (Player player in Rounds.Last().Matches.SelectMany(match => match.Players))
            {
                Rounds.Last().Players.Add(player);
            }

            Rounds.Last().Matches.Clear();
        }
    }
}
