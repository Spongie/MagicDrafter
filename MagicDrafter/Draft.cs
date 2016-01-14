using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows;

namespace MagicDrafter
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

        public bool Done { get; set; }

        internal List<Player> getTemporaryStandings()
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

        public List<Player> getFinalStandings()
        {
            var players = PairingUtility.GetPlayersOrdered(ivAllMatches, ivPlayers.ToList());

            for (int i = 0; i < players.Count; i++)
            {
                players[i].Rank = i + 1;
            }

            return players;
        }

        internal bool Start()
        {
            ivRounds = new ObservableCollection<Round>();
            ivAllMatches = new List<Match>();

            if (ivPlayers.Count % 2 != 0)
                ivPlayers.Add(new Player("Bye"));

            StartNextRound(false);
            return true;
        }

        public void StartNextRound(bool sendRoundFinished = true)
        {
            if(ivRounds.Any())
                ivAllMatches.AddRange(ivRounds.Last().Matches);

            if (sendRoundFinished)
                OnRoundFinished(this, new EventArgs());

            ivRounds.Add(new Round(ivAllMatches, ivPlayers.ToList(), ivRounds.Count + 1));

            OnNewRoundStart(this, new EventArgs());
        }

        public void FinishDraft()
        {
            Done = true;
            ivAllMatches.AddRange(ivRounds.Last().Matches);
            OnRoundFinished(this, new EventArgs());
        }

        internal void RegisterScore(int player1Score, int player2Score)
        {
            SelectedMatch.RegisterScore(player1Score, player2Score);
            FirePropertyChanged("SelectedMatch");
            FirePropertyChanged("Rounds");
        }
    }
}
