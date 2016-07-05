using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MagicDrafterCore;

namespace MagicDrafter
{
    /// <summary>
    /// Interaction logic for ManualParingWindow.xaml
    /// </summary>
    public partial class ManualParingWindow : Window
    {
        public Round Round;

        public ManualParingWindow(Round piRound)
        {
            Round = piRound;
            DataContext = Round;
            InitializeComponent();
        }

        public ManualParingWindow()
        {
            InitializeComponent();
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedMatch = ivListMatches.SelectedItem as Match;

            if (selectedMatch != null)
            {
                Round.Players.Add(selectedMatch.Players.First());
                Round.Players.Add(selectedMatch.Players.Last());
                Round.Matches.Remove(selectedMatch);
            }

            ivButtonConfirm.IsEnabled = !Round.Players.Any();
        }

        private void ButtonCreateMatch_OnClick(object sender, RoutedEventArgs e)
        {
            if (ivListPlayers.SelectedItems.Count != 2)
            {
                MessageBox.Show("2 Players must be selected to create a match");
                return;
            }

            var players = ivListPlayers.SelectedItems.Cast<Player>().ToList();

            Round.Matches.Add(new Match(players.First(), players.Last()));
            Round.Players.Remove(players.First());
            Round.Players.Remove(players.Last());

            ivButtonConfirm.IsEnabled = !Round.Players.Any();
        }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
