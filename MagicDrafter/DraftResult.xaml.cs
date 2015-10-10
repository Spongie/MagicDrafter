using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MagicDrafter
{
    /// <summary>
    /// Interaction logic for DraftResult.xaml
    /// </summary>
    public partial class DraftResult : UserControl
    {
        public DraftResult()
        {
            InitializeComponent();
        }

        public void SetPlayers(List<Player> piPlayers)
        {
            listboxResult.DataContext = piPlayers;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var players = (List<Player>)listboxResult.DataContext;
            var json = JsonConvert.SerializeObject(players);

            File.WriteAllText(string.Format("Draft_{0}.xml", DateTime.Now.ToShortDateString()), json);
        }
    }
}
