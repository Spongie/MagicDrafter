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
        public event EventHandler OnDraftFinished;

        public Draft()
        {
            ivPlayers = new ObservableCollection<Player>();
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
            if(ivPlayers.Count < 5)
            {
                MessageBox.Show("Draft must contain atleast 5 people", "Error!");
                return false;
            }

            ivRounds = new ObservableCollection<Round>();
            ivAllMatches = new List<Match>();

            if (ivPlayers.Count % 2 != 0)
                ivPlayers.Add(new Player("Bye"));

            StartNextRound();
            return true;
        }

        public void StartNextRound()
        {
            if(ivRounds.Any())
                ivAllMatches.AddRange(ivRounds.Last().Matches);

            ivRounds.Add(new Round(ivAllMatches, ivPlayers.ToList(), ivRounds.Count + 1));

            OnNewRoundStart(this, new EventArgs());
        }

        public void FinishDraft()
        {
            ivAllMatches.AddRange(ivRounds.Last().Matches);
            OnDraftFinished(this, new EventArgs());
        }

        internal void RegisterScore(int player1Score, int player2Score)
        {
            SelectedMatch.RegisterScore(player1Score, player2Score);
            FirePropertyChanged("SelectedMatch");
            FirePropertyChanged("Rounds");
        }
    }
}
