using SuperFarmer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
    /// Represents the active game window. 
    /// Handles player interactions, displays game state updates, and allows actions like dice rolls and animal exchanges.
    /// </summary>

    public partial class ActiveGame : Window
    {
        private Player currentPlayer;
        private SuperFarmerGame game;
        private GameBox gameBox;
        private List<System.Windows.Controls.Label> playerLabels;
        private List<System.Windows.Controls.Label> playerLabelsBoard;

        public ActiveGame(SuperFarmerGame game)
        {
            InitializeComponent();
            this.game = game;
            Label1.BorderBrush = Brushes.White;
            Label1.BorderThickness = new Thickness(2);
            DataContext = this;

            this.gameBox = new GameBox();

            playerLabels = new List<System.Windows.Controls.Label> { LabelPlayer1, LabelPlayer2, LabelPlayer3, LabelPlayer4 };
            playerLabelsBoard = new List<System.Windows.Controls.Label> { Label1, Label2, Label3, Label4 };


            if (game.Players.Count == 2)
            {
                Name1.Content = "Player 1: " + game.Players[0].Name;
                Name2.Content = "Player 2: " + game.Players[1].Name;
            }
            else if (game.Players.Count == 3)
            {
                Name1.Content = "Player 1: " + game.Players[0].Name;
                Name2.Content = "Player 2: " + game.Players[1].Name;
                Name3.Content = "Player 3: " + game.Players[2].Name;
            }
            else
            {
                Name1.Content = "Player 1: " + game.Players[0].Name;
                Name2.Content = "Player 2: " + game.Players[1].Name;
                Name3.Content = "Player 3: " + game.Players[2].Name;
                Name4.Content = "Player 4: " + game.Players[3].Name;
            }
            foreach (Player player in game.Players)
            {
                game.PropertyChanged += Game_PropertyChanged;
            }

        }

        private void Game_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdatePlayerLabels();
        }

        private int currentPlayerIndex = 0;
        private Player GetThePlayer()
        {

            if (currentPlayerIndex >= game.Players.Count)
            {
                currentPlayerIndex = 0;
            }

            return game.Players[currentPlayerIndex];
        }
        private int UpdatePlayerIndex()
        {
            currentPlayerIndex++;
            return currentPlayerIndex;
        }

        private void UpdatePlayerLabels()
        {
           
            try
            {
                for (int i = 0; i < game.Players.Count; i++)
                {
                    string playerName = game.Players[i].Name;
                    playerLabels[i].Content = $"{GetHerdStateText(game.Players[i].GetHerd())}";
                    lblBox.Content = $"{GetGameBoxText()}";


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd w UpdatePlayerLabels: {ex.Message}");
            }
        }
        private string GetHerdStateText(Dictionary<EnumAnimal, int> herdState)
        {
            if (herdState == null)
            {
                herdState = new Dictionary<EnumAnimal, int>();
            }
            StringBuilder sb = new();
            foreach (KeyValuePair<EnumAnimal, int> kvp in herdState)
            {
                if (kvp.Key.ToString() == "Wolf" || kvp.Key.ToString() == "Fox")
                    continue;
                sb.Append($"{kvp.Key}: {kvp.Value} \n");
            }
            return sb.ToString();
        }

        private string GetGameBoxText()
        {
            Dictionary<EnumAnimal, int> AnimalCards = this.game.GetAnimalCards();
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in AnimalCards)
            {
                
               
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            return sb.ToString();
        }

        private void Game_AnimalCardsUpdated(object sender, EventArgs e)
        {
            UpdatePlayerLabels();
        }

        // Handles the roll button click event. Rolls the dice for the current player and updates the UI.
        private void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            currentPlayer = GetThePlayer();
            var diceResult = game.Dice.RollAnimalDice();
            Roll roll = new(currentPlayer, diceResult, game, LabelStatus);
            roll.AnimalCardsUpdated += Game_AnimalCardsUpdated;
            UpdatePlayerIndex();
            UpdatePlayerLabels();
            int index = (currentPlayerIndex) % game.Players.Count;
            playerLabelsBoard[currentPlayerIndex-1].BorderThickness = new Thickness(0);
            playerLabelsBoard[index].BorderBrush = Brushes.White;
            playerLabelsBoard[index].BorderThickness = new Thickness(4);
        }

        private void btnExchange_Click(object sender, RoutedEventArgs e)
        {
            currentPlayer = GetThePlayer();
            Exchange exchange = new Exchange(currentPlayer, game);
            exchange.AnimalCardsUpdated += Game_AnimalCardsUpdated;
            exchange.Show();
        }

    }

}
