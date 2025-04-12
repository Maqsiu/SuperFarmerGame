using SuperFarmer;
using System;
using System.Windows;


namespace GUI_SuperFarmer
{
    /// <summary>
    /// Represents the Exchange window where players can perform trades in the game.
    /// </summary>
    public partial class Exchange : Window
    {
        private Player player;
        private SuperFarmerGame game;
        public event EventHandler AnimalCardsUpdated;
        private int option;

        public Exchange(Player player, SuperFarmerGame game)
        {
            InitializeComponent();
            InitializeTypeOfExchange();
            this.player = player;
            this.game = game;
        }

        // Initializes the types of exchanges available in the game.
        private void InitializeTypeOfExchange()
        {
            CB_Exchange.Items.Add("1. Exchange 6 rabbits for 1 sheep");
            CB_Exchange.Items.Add("2. Exchange 2 sheep for 1 pig");
            CB_Exchange.Items.Add("3. Exchange 3 pigs for 1 cow");
            CB_Exchange.Items.Add("4. Exchange 2 cows for 1 horse");
            CB_Exchange.Items.Add("5. Exchange 1 sheep for 1 small dog");
            CB_Exchange.Items.Add("6. Exchange 1 cow for 1 big dog");
            CB_Exchange.Items.Add("7. Exchange 1 sheep for 6 rabbits");
            CB_Exchange.Items.Add("8. Exchange 1 pig for 2 sheep");
            CB_Exchange.Items.Add("9. Exchange 1 cow for 3 pigs");
            CB_Exchange.Items.Add("10. Exchange 1 horse for 2 cows");
            CB_Exchange.Items.Add("11. Exchange 1 small dog for 1 sheep");
            CB_Exchange.Items.Add("12. Exchange 1 big dog for 1 cow");

        }

        protected virtual void OnAnimalCardsUpdated()
        {
            AnimalCardsUpdated?.Invoke(this, EventArgs.Empty);
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (CB_Exchange.SelectedIndex != -1)
            {
                string selectedOption = CB_Exchange.SelectedItem.ToString();

                string[] parts = selectedOption.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0 && int.TryParse(parts[0], out option))
                {
                    game.PerformTrade(player, option);
                    OnAnimalCardsUpdated();
                }
                else
                {
                    MessageBox.Show("You don't have enough animals to exchange or there aren't enough animals in the box!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            OnAnimalCardsUpdated();
            this.Close();
        }
    }
}
