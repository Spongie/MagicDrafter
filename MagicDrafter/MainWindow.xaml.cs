using System;
using System.Windows;
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
        }

        private void OnNewRoundStart(object sender, EventArgs e)
        {
            tabControl.SelectedIndex++;

            switch (tabControl.SelectedIndex)
            {
                case 1:
                    tabRound1.IsEnabled = true;
                    matchViewRound1.TheDraft = ivDraft;
                    matchViewRound1.SetRound(0);
                    break;
                case 2:
                    tabRound2.IsEnabled = true;
                    matchViewRound2.TheDraft = ivDraft;
                    matchViewRound2.SetRound(1);
                    break;
                case 3:
                    tabRound3.IsEnabled = true;
                    matchViewRound3.TheDraft = ivDraft;
                    matchViewRound3.SetRound(2);
                    break;
                default:
                    break;
            }
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
            if(ivDraft.Start())
                matchViewRound1.matchDataGrid.SelectedIndex = 0;
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
