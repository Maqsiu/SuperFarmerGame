using SuperFarmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

namespace GUI_SuperFarmer
{
    /// <summary>
    /// NumOfPlayers class for selecting the number of players in the Super Farmer game
    /// This class is a UserControl that allows users to choose the number of players for the game.
    /// </summary>


    public partial class NumOfPlayers : UserControl
    {

        SuperFarmerGame game;
        List<Player> playerList;
        public int numOfPlayers = 0;
        private MainWindow _windowsStart;



        public NumOfPlayers(MainWindow windowStart)
        {
            InitializeComponent();
            game = new SuperFarmerGame();
            playerList = new List<Player>();
            _windowsStart = windowStart;
        }

        private void Players2_btn_Click(object sender, RoutedEventArgs e)
        {
                    
                numOfPlayers = 2;

            _windowsStart.Content = new UserNameAdd(numOfPlayers);

        }
        private void Players3_btn_Click(object sender, RoutedEventArgs e)
        {

            numOfPlayers = 3;

            _windowsStart.Content = new UserNameAdd(numOfPlayers);

        }
        private void Players4_btn_Click(object sender, RoutedEventArgs e)
        {

            numOfPlayers = 4;

            _windowsStart.Content = new UserNameAdd(numOfPlayers);

        }

    }
}
