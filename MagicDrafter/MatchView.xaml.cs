using System;
using System.Windows;
using System.Windows.Controls;

namespace MagicDrafter
{
    /// <summary>
    /// Interaction logic for MatchView.xaml
    /// </summary>
    public partial class MatchView : UserControl
    {
        private int ivRound;

        public event EventHandler onMatchReported;

        public MatchView()
        {
            InitializeComponent();
        }

        public Draft TheDraft { get; set; }

        public void SetRound(int round)
        {
            ivRound = round;
            matchDataGrid.ItemsSource = TheDraft.Rounds[round].Matches;

            buttonStartNextRound.Visibility = ivRound != 2 ? Visibility.Visible : Visibility.Hidden;
            buttonViewResult.Visibility = ivRound == 2 ? Visibility.Visible : Visibility.Hidden;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TheDraft.SelectedMatch = (Match)e.AddedItems[0];
                StackPlayerInfo.DataContext = TheDraft.SelectedMatch;
            }
        }

        private void buttonStartNextRound_Click(object sender, RoutedEventArgs e)
        {
            TheDraft.StartNextRound();
            buttonStartNextRound.IsEnabled = false;
        }

        private void RefreshItemSource()
        {
            matchDataGrid.ItemsSource = null;
            matchDataGrid.ItemsSource = TheDraft.Rounds[ivRound].Matches;
            buttonStartNextRound.IsEnabled = TheDraft.Rounds[ivRound].IsRoundReported();
            buttonViewResult.IsEnabled = TheDraft.Rounds[ivRound].IsRoundReported();
        }

        private void Button_Click20(object sender, RoutedEventArgs e)
        {
            TheDraft.SelectedMatch.RegisterScore(2, 0);
            RefreshItemSource();

            FireMatchReported();
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            TheDraft.SelectedMatch.RegisterScore(2, 1);
            RefreshItemSource();

            FireMatchReported();
        }

        private void Button_Click_02(object sender, RoutedEventArgs e)
        {
            TheDraft.SelectedMatch.RegisterScore(0, 2);
            RefreshItemSource();

            FireMatchReported();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            TheDraft.SelectedMatch.RegisterScore(1, 2);
            RefreshItemSource();

            FireMatchReported();
        }

        private void Button_Click11(object sender, RoutedEventArgs e)
        {
            TheDraft.SelectedMatch.RegisterScore(1, 1);
            RefreshItemSource();

            FireMatchReported();
        }

        private void Button_Click10(object sender, RoutedEventArgs e)
        {
            TheDraft.SelectedMatch.RegisterScore(1, 0);
            RefreshItemSource();

            FireMatchReported();
        }

        private void Button_Click01(object sender, RoutedEventArgs e)
        {
            TheDraft.SelectedMatch.RegisterScore(0, 1);
            RefreshItemSource();

            FireMatchReported();
        }

        private void buttonViewResult_Click(object sender, RoutedEventArgs e)
        {
            TheDraft.FinishDraft();
            buttonViewResult.IsEnabled = false;

            FireMatchReported();
        }

        private void FireMatchReported()
        {
            if (onMatchReported != null)
                onMatchReported(this, new EventArgs());
        }
    }
}
