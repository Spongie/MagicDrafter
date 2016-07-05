using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MagicDrafterCore;

namespace MagicDrafter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Draft ivDraft;

        public MainWindow()
        {
            ivDraft = new Draft();
            InitializeComponent();
            ivDraft.OnNewRoundStart += OnNewRoundStart;
            ivDraft.OnRoundFinished += OnRoundFinished;

            DataContext = ivDraft;
        }

        private void OnRoundFinished(object sender, EventArgs e)
        {
            tabResult.IsEnabled = true;
            draftResult.DataContext = ivDraft;
            draftResult.SetPlayers(ivDraft.GetFinalStandings().ToList());

            if (ivDraft.Done)
                tabControl.SelectedIndex = tabControl.Items.Count - 1;
        }

        private void OnNewRoundStart(object sender, EventArgs e)
        {
            var tabItem = new TabItem { Header = "Round " + ivDraft.Rounds.Count };

            var matchView = new MatchView { TheDraft = ivDraft };
            matchView.SetRound(ivDraft.Rounds.Count - 1);
            matchView.onMatchReported += MatchView_onMatchReported;

            tabItem.Content = matchView;
            tabControl.Items.Insert(tabControl.Items.Count - 1, tabItem);
            tabControl.SelectedIndex++;
        }

        private void MatchView_onMatchReported(object sender, EventArgs e)
        {
            tabResult.IsEnabled = true;
            draftResult.DataContext = ivDraft;
            draftResult.SetPlayers(ivDraft.GetTemporaryStandings().ToList());
        }

        private void button_AddPlayerClick(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBoxPlayerName.Text))
            {
                MessageBox.Show("Name cannot be empty!");
                return;
            }

            if (ivDraft.Players.Any(player => player.Name == textBoxPlayerName.Text))
            {
                MessageBox.Show("A player with the same name is already in the draft!");
                return;
            }

            ivDraft.Players.Add(new Player(textBoxPlayerName.Text));
            textBoxPlayerName.Clear();
        }

        private void buttonStartDraft_Click(object sender, RoutedEventArgs e)
        {
            ivDraft.Start(false);
            buttonStartDraft.IsEnabled = false;
            buttonStartDraftManual.IsEnabled = false;
        }

        private void textBoxPlayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                button_AddPlayerClick(null, new RoutedEventArgs());
            }
        }

        private void ButtonStartDraftManual_OnClick(object sender, RoutedEventArgs e)
        {
            ivDraft.Start(true);
            var pairingWindow = new ManualParingWindow(ivDraft.Rounds.Last());

            var result = pairingWindow.ShowDialog();

            if (result.HasValue && result.Value)
            {
                ivDraft.ConfirmManualPairing();
                buttonStartDraftManual.IsEnabled = false;
                buttonStartDraft.IsEnabled = false;
            }
        }
    }
}
