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


namespace GUI_SuperFarmer
{

    /// <summary>
    /// UserNameAdd class manages the player name input and initializes the players for the game.
    /// This class is responsible for handling player name input, updating the player list, and moving on to the active game screen.
    /// </summary>

    public partial class UserNameAdd : UserControl
    {

        SuperFarmerGame game;
        List<Player> playerList;


        public int _numOfPlayers = 0;

        private List<System.Windows.Controls.TextBox> playerTextBoxBoard;
        private List<System.Windows.Controls.Label> playerTextLabel;
        private List<System.Windows.Controls.Border> playerTextBorder;
        public UserNameAdd(int numOfPlayers)
        {
            game = new SuperFarmerGame();
            playerList = new List<Player>();
            InitializeComponent();
            _numOfPlayers = numOfPlayers;
            playerTextBorder = new List<System.Windows.Controls.Border> { Panel1, Panel2, Panel3, Panel4 };
            playerTextLabel = new List<System.Windows.Controls.Label> { player1Label, player2Label, player3Label, player4Label };
            playerTextBoxBoard = new List<System.Windows.Controls.TextBox> { player1Name, player2Name, player3Name, player4Name };
            for (int i = 0; i < numOfPlayers; i++)
            {
                playerTextBoxBoard[i].Visibility = Visibility.Visible;
                playerTextBorder[i].Visibility = Visibility.Visible;
                playerTextLabel[i].Visibility = Visibility.Visible;
            }
        }


        private void ConfirmSec_btn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _numOfPlayers; i++)
            {
                string playerName = playerTextBoxBoard[i].Text;

                if (!string.IsNullOrEmpty(playerName))
                {
                    Player newPlayer = new Player();
                    newPlayer.Name = playerName;
                    playerList.Add(newPlayer);
                }
            }
            game.Players = playerList;
            ActiveGame activeGame = new ActiveGame(game);
            activeGame.Show();
            
        }

    }
}
