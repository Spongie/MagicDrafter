using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MagicDrafter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Draft ivDraft;

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
            draftResult.SetPlayers(ivDraft.getFinalStandings());

            if (ivDraft.Done)
                tabControl.SelectedIndex = tabControl.Items.Count - 1;
        }

        private void OnNewRoundStart(object sender, EventArgs e)
        {
            var tabItem = new TabItem();
            tabItem.Header = "Round " + ivDraft.Rounds.Count;
            
            var matchView = new MatchView();
            matchView.TheDraft = ivDraft;
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
            draftResult.SetPlayers(ivDraft.getTemporaryStandings());
        }

        private void button_AddPlayerClick(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBoxPlayerName.Text))
            {
                MessageBox.Show("Name cannot be empty!");
                return;
            }

            ivDraft.Players.Add(new Player(textBoxPlayerName.Text));
            textBoxPlayerName.Clear();
        }

        private void buttonStartDraft_Click(object sender, RoutedEventArgs e)
        {
            ivDraft.Start();
        }

        private void textBoxPlayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                button_AddPlayerClick(null, new RoutedEventArgs());
            }
        }
    }
}
