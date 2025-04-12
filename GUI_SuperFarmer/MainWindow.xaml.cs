using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI_SuperFarmer
{
    /// <summary>
    /// This class represents the main window of the application and handles user interactions.
    /// </summary>


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnHowToPlay_Click(object sender, RoutedEventArgs e)
        {
            String howToPlay = "You are an animal breeder and want to become a super farmer. Your animals breed, which brings you profit. You can exchange the animals you breed for others if you think it's profitable. To win, you must be the first to have a herd consisting of at least one horse, cow, pig, sheep, and rabbit. However, all your plans could remain just dreams if you're not careful! Wolves and foxes roam the area, and your animals could easily become their prey.\r\n*Game Progress:*\r\nThe game can involve 2 to 4 players. One additional person, if they want, can participate as the herd caretaker and only perform exchanges without actively playing. All tokens and dog figurines are in the main herd, which is best kept in the box. Each player receives one rabbit, which they place on the table in front of them.\r\nBreeding Animals\r\nPlayers take turns, always rolling two dice. If a player rolls a pair of the same animal, they get that animal from the main herd. When a player already has animals, after each roll, they receive as many animals of the rolled type from the herd as they have complete pairs of that type (including those rolled on the dice).\r\nEXAMPLES:\r\n1. If a player had 6 rabbits and 1 pig, and rolled a rabbit and a pig, they get 3 rabbits and 1 pig.\r\n2. If the player had 6 rabbits and 1 pig, and rolled a sheep and a pig, they only get 1 pig.\r\n3. If the player had 5 rabbits and 1 cow, and rolled a sheep and a pig, they get nothing.\r\n4. If the player had 4 rabbits, 2 sheep, and 1 horse, and rolled 2 pigs, they receive 1 pig from the main herd.\r\nNOTE! A player who does not have a horse or cow cannot receive such animals from a roll, as one die shows a horse, and the other shows a cow.\r\nThe first horse or cow can only be obtained through exchange.\r\nExchange\r\nBefore each dice roll, a player may choose to perform one exchange: with the main herd (if the herd has the necessary animals) or with another player (if they agree).\r\nExchanges are made according to the conversion rates shown in the exchange table.\r\nNOTE! A player can exchange several animals for one animal. They can also exchange one animal for several animals, following the rates shown in the exchange table.\r\nBargaining is not allowed, and players cannot ask for more than the table suggests. If the herd has few animals left, the player will receive only as many as are available from the herd (through a dice roll or exchange), losing the right to the missing animals. For example, if there are only 3 rabbits left in the herd, and the player should receive 4 rabbits from the dice roll, they will only get 3.\r\nEXAMPLES:\r\n1. A player who has 6 rabbits, 1 sheep, and 2 pigs may exchange them for 1 cow (because 6 rabbits equal 1 sheep, 2 sheep equal 1 pig, and 3 pigs equal 1 cow).\r\n2. A player with 1 horse can exchange it for, for example, 1 cow, 2 pigs, and 2 sheep.\r\n3. A player with 6 rabbits and 2 cows cannot exchange all 6 rabbits for sheep and 2 cows for a horse in one exchange.\r\n7. Loss of Animals\r\nIf a player rolls a fox, they lose all their rabbits except one to the herd. If they roll a wolf, they lose all their animals except for the horse, rabbits, and small dog (if they have one).\r\nEXAMPLES:\r\nIf a player rolls a wolf and a fox, has a small dog, but no large dog, they lose all their sheep, pigs, and cows. The horse and rabbits remain safe. Then, the small dog is placed back in the box. The small dog protects the player from the fox's loss.\r\nIf the player rolls a fox and has a small dog, their animals remain untouched; only the small dog returns to the herd.\r\nThe large dog protects against the wolf's loss. If the player rolls a wolf and has a large dog, their animals remain safe; only the large dog returns to the herd.\r\nNOTE! The large dog does not protect against the fox.\r\nEnd of the Game\r\nThe player who becomes the Super Farmer is the first to have at least one horse, cow, pig, sheep, and rabbit in their herd. The others can continue playing.";
            MessageBox.Show(howToPlay, "How to play? ");
        }

        private void BtnStartAGame_Click(object sender, RoutedEventArgs e)
        {
            NumOfPlayers setPlayers = new NumOfPlayers(this);
            MainContent.Content = new GUI_SuperFarmer.NumOfPlayers(this);
            
        }
    }
}
