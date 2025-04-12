using SuperFarmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Logika interakcji dla klasy Player1.xaml
    /// </summary>
    public partial class Player1 : Window
    {
        private Player currentPlayer;
        private SuperFarmerGame game;
        public Player1(Player player)
        {
            InitializeComponent();
            currentPlayer = player;
            Title = $"{currentPlayer.Name}";
        }
        //private Player player;
        private (EnumAnimal, EnumAnimal) diceResult;
        private void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            // Obsługa kliknięcia przycisku "Roll the Dice"
            var diceResult = game.Dice.RollAnimalDice();
            Roll roll = new(currentPlayer, diceResult, game);
            roll.Show();
            //Close();
        }


        private void btnExchange_Click(object sender, RoutedEventArgs e)
        {
            // Obsługa kliknięcia przycisku "Exchange"
            // Tutaj możesz otworzyć okno wymiany
            // Np. otworzyć nowe okno z opcjami wymiany
            Close();
        }
    }
}
