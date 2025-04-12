using SuperFarmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI_SuperFarmer
{

    /// <summary>
    /// Roll class handles the rolling of dice, updates the player's herd, and displays the result.
    /// This class is responsible for managing the results of the dice roll, updating the player's herd,
    /// and triggering the necessary UI updates.
    /// </summary>
    class Roll
    {
        private Player player;
        private (EnumAnimal, EnumAnimal) diceResult;
        private GameBox box;
        private SuperFarmerGame game;
        public event EventHandler AnimalCardsUpdated;

        public Roll(Player player, (EnumAnimal, EnumAnimal) diceResult, SuperFarmerGame game, Label statusPlayer)
        {
            this.player = player;
            this.diceResult = diceResult;
            this.game = game;
            statusPlayer.Content = "";
            ShowDropFor2Seconds(statusPlayer);

            game.UpdateHerd(player, diceResult);
            OnAnimalCardsUpdated();
        }
        private async void ShowDropFor2Seconds(Label statusPlayer)
        {
            statusPlayer.Content = "Your drop: \n" + diceResult.Item1 + "\n&\n" + diceResult.Item2;


            await Task.Delay(2000);


            statusPlayer.Content = "";
        }

        protected virtual void OnAnimalCardsUpdated()
        {
            AnimalCardsUpdated?.Invoke(this, EventArgs.Empty);
        }

    }
}
