using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

namespace SuperFarmer
{
    /// <summary>
    /// The class represents the core logic for the "Super Farmer" game. 
    /// It handles the main game loop, player interactions, and trade mechanics between animals. 
    /// This class is part of the initial console version of the game, providing gameplay features such as:
    /// - Managing players and their animal herds.
    /// - Handling trades between animals using predefined trade ratios.
    /// - Simulating dice rolls to determine animal outcomes.
    /// - Enabling players to engage in trades or roll the dice for their turn.
    /// The game continues until a player collects all required animals to win, with each round offering players 
    /// opportunities to trade or roll the dice. The class also tracks the progress of the game and stores the 
    /// results at the end of the session.
    /// </summary>
    
    public class SuperFarmerGame : GameBox, INotifyPropertyChanged
    {
        private List<Player> players;
        private Dice dice;
        private Dictionary<(EnumAnimal, EnumAnimal), (int, int)> tradeRatios;

        public event PropertyChangedEventHandler? PropertyChanged;


        public List<Player> Players { get => players; set => players = value; }
        public Dice Dice { get => dice; set => dice = value; }
        public Dictionary<(EnumAnimal, EnumAnimal), (int, int)> TradeRatios { get => tradeRatios; set => tradeRatios = value; }



        public SuperFarmerGame()
        {
            this.players = new List<Player>();
            this.dice = new Dice();

            this.tradeRatios = new Dictionary<(EnumAnimal, EnumAnimal), (int, int)>
            {
                { (EnumAnimal.Rabbit, EnumAnimal.Sheep), (6,1) },
                { (EnumAnimal.Sheep, EnumAnimal.Pig), (2,1) },
                { (EnumAnimal.Pig, EnumAnimal.Cow), (3,1) },
                { (EnumAnimal.Cow, EnumAnimal.Horse), (2, 1) },
                { (EnumAnimal.Sheep, EnumAnimal.SmallDog), (1,1)},
                { (EnumAnimal.Cow, EnumAnimal.BigDog), (1 ,1)},
                { (EnumAnimal.Sheep, EnumAnimal.Rabbit), (1,6) },
                { (EnumAnimal.Pig, EnumAnimal.Sheep), (1,2) },
                { (EnumAnimal.Cow, EnumAnimal.Pig), (1,3) },
                { (EnumAnimal.Horse, EnumAnimal.Cow), (1,2) },
                { (EnumAnimal.SmallDog, EnumAnimal.Sheep), (1,1) },
                { (EnumAnimal.BigDog, EnumAnimal.Cow), (1,1) },
            };
        }


        private (int, int) GetTradeRatio(EnumAnimal fromAnimal, EnumAnimal toAnimal)
        {
            // Check if a trade ratio exists for the given exchange
            if (tradeRatios.TryGetValue((fromAnimal, toAnimal), out var ratio))
            {
                return ratio;
            }

            // If not, return a default 1:1 ratio
            return (1, 1);
        }
        public void StartTrade(Player player)
        {
            Console.WriteLine($"{player.Name}, you want to make an exchange. Choose an option:");

            // Example exchanges - you can adjust as needed
            Console.WriteLine("1. Exchange 6 rabbits for 1 sheep");
            Console.WriteLine("2. Exchange 2 sheep for 1 pig");
            Console.WriteLine("3. Exchange 3 pigs for 1 cow");
            Console.WriteLine("4. Exchange 2 cows for 1 horse");
            Console.WriteLine("5. Exchange 1 sheep for 1 small dog");
            Console.WriteLine("6. Exchange 1 cow for 1 big dog");
            Console.WriteLine("7. Exchange 1 sheep for 6 rabbits");
            Console.WriteLine("8. Exchange 1 pig for 2 sheep");
            Console.WriteLine("9. Exchange 1 cow for 3 pigs");
            Console.WriteLine("10. Exchange 1 horse for 2 cows");
            Console.WriteLine("11. Exchange 1 small dog for 1 sheep");
            Console.WriteLine("12. Exchange 1 big dog for 1 cow");

            int option = int.Parse(Console.ReadLine());

            // Check if the option is valid
            if (option < 1 || option > 12)
            {
                Console.WriteLine("Invalid option.");
                return;
            }
            gameBoxContents();
            // Execute the first exchange
            PerformTrade(player, option);


            // Continue the exchange
            while (true)
            {
                Console.WriteLine(player);
                Console.WriteLine($"{player.Name}, do you want to continue exchanging? Type 'Yes' or 'No':");
                string choice = Console.ReadLine().ToLower();

                if (choice == "no")
                {
                    Console.WriteLine($"{player.Name}, ending exchange. You can now roll the dice.");
                    var diceResult = dice.RollAnimalDice();
                    UpdateHerd(player, diceResult);
                    Console.WriteLine(player);
                    break;
                }


                // Choose a new exchange option
                Console.WriteLine($"{player.Name}, choose a new exchange option (1-12):");
                if (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 12)
                {
                    Console.WriteLine("Invalid option.");
                    return;
                }

                Console.WriteLine($"{player.Name}, make the next exchange.");

                // Execute the next exchange
                PerformTrade(player, option);
            }
        }


        public void PerformTrade(Player player, int option)
        {
            // Define trade mappings
            var tradeOptions = new Dictionary<int, (EnumAnimal fromAnimal, EnumAnimal toAnimal, (int, int) ratio)>
    {
        { 1, (EnumAnimal.Rabbit, EnumAnimal.Sheep, (1, 6)) },
        { 2, (EnumAnimal.Sheep, EnumAnimal.Pig, (1, 2)) },
        { 3, (EnumAnimal.Pig, EnumAnimal.Cow, (1, 3)) },
        { 4, (EnumAnimal.Cow, EnumAnimal.Horse, (1, 2)) },
        { 5, (EnumAnimal.Sheep, EnumAnimal.SmallDog, (1, 1)) },
        { 6, (EnumAnimal.Cow, EnumAnimal.BigDog, (1, 1)) },
        { 7, (EnumAnimal.Sheep, EnumAnimal.Rabbit, (1, 6)) },
        { 8, (EnumAnimal.Pig, EnumAnimal.Sheep, (1, 2)) },
        { 9, (EnumAnimal.Cow, EnumAnimal.Pig, (1, 3)) },
        { 10, (EnumAnimal.Horse, EnumAnimal.Cow, (1, 2)) },
        { 11, (EnumAnimal.SmallDog, EnumAnimal.Sheep, (1, 1)) },
        { 12, (EnumAnimal.BigDog, EnumAnimal.Cow, (1, 1)) }
    };

            // Check if the option is valid
            if (tradeOptions.ContainsKey(option))
            {
                var trade = tradeOptions[option];
                if (player.TradePossibility(trade.fromAnimal, trade.toAnimal, trade.ratio))
                {
                    player.TradeWithMainHerd(trade.fromAnimal, trade.toAnimal, trade.ratio);
                    IncreaseAnimalCardCount(trade.fromAnimal, trade.ratio.Item1);
                    DecreaseAnimalCardCount(trade.toAnimal, trade.ratio.Item2);
                }
                else
                {
                    Console.WriteLine($"You do not have enough {trade.fromAnimal} for the trade, or there aren't enough {trade.toAnimal} in the box!");
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }

        public void AddPlayer(string playerName)
        {
            this.players.Add(new Player(playerName));
        }

        private void FoxDo(Player player)
        {
            if (player.HasAnimal(EnumAnimal.SmallDog))
            {
                player.RemoveFromHerd(EnumAnimal.SmallDog, 1);
                IncreaseAnimalCardCount(EnumAnimal.SmallDog, 1);
                Console.WriteLine("Returning SmallDog to the box.");
            }
            // Handle the fox throw
            else
            {
                int rabbitCount = player.GetHerdCount(EnumAnimal.Rabbit);
                if (rabbitCount > 0)
                {
                    player.RemoveFromHerd(EnumAnimal.Rabbit, rabbitCount - 1);
                    IncreaseAnimalCardCount(EnumAnimal.Rabbit, rabbitCount - 1);
                    Console.WriteLine($"Returning {rabbitCount} Rabbits to the box.");
                }
            }
        }

        private void WolfDo(Player player)
        {
            if (player.HasAnimal(EnumAnimal.BigDog))
            {
                player.RemoveFromHerd(EnumAnimal.BigDog, 1);
                IncreaseAnimalCardCount(EnumAnimal.BigDog, 1);
                Console.WriteLine("Returning BigDog to the box.");
            }
            else
            {
                // Handle the wolf throw
                int sheepCount = player.GetHerdCount(EnumAnimal.Sheep);
                if (sheepCount > 0)
                {
                    Console.WriteLine($"Returning {sheepCount} Sheep to the box.");
                    player.RemoveFromHerd(EnumAnimal.Sheep, sheepCount);
                    IncreaseAnimalCardCount(EnumAnimal.Sheep, sheepCount);
                }
                int pigCount = player.GetHerdCount(EnumAnimal.Pig);
                if (pigCount > 0)
                {
                    Console.WriteLine($"Returning {pigCount} Pigs to the box.");
                    player.RemoveFromHerd(EnumAnimal.Pig, pigCount);
                    IncreaseAnimalCardCount(EnumAnimal.Pig, pigCount);
                }
                int cowCount = player.GetHerdCount(EnumAnimal.Cow);
                if (cowCount > 0)
                {
                    Console.WriteLine($"Returning {cowCount} Cows to the box.");
                    player.RemoveFromHerd(EnumAnimal.Cow, cowCount);
                    IncreaseAnimalCardCount(EnumAnimal.Cow, cowCount);
                }
            }
        }

        public void UpdateHerd(Player player, (EnumAnimal, EnumAnimal) diceResult)
        {
            EnumAnimal animal1 = diceResult.Item1;
            EnumAnimal animal2 = diceResult.Item2;
            if (animal2 == EnumAnimal.Fox && animal1 == EnumAnimal.Wolf)
            {
                FoxDo(player);
                WolfDo(player);
            }
            else if (animal2 == EnumAnimal.Fox)
            {
                FoxDo(player);
            }
            else if (animal1 == EnumAnimal.Wolf)
            {
                WolfDo(player);
            }
            else
            {
                UpdatePlayerHerd(player, diceResult);
            }
        }


        private void UpdatePlayerHerd(Player player, (EnumAnimal, EnumAnimal) diceResult)
        {
            try
            {
                EnumAnimal animal1 = diceResult.Item1;
                EnumAnimal animal2 = diceResult.Item2;
                if (player.GetHerdCount(animal1) > 0)
                {
                    int animalcount = player.GetHerdCount(animal1);
                    int pairToAdd = animalcount / 2;
                    int dodanie1;
                    if (animalcount % 2 == 0)
                    {
                        if (animal1 == animal2)
                        {
                            if (GetAnimalCardCount(animal1) >= pairToAdd + 1)
                            {
                                dodanie1 = pairToAdd + 1;
                            }
                            else
                            {
                                dodanie1 = GetAnimalCardCount(animal1);
                            }

                        }
                        else
                        {
                            if (GetAnimalCardCount(animal1) >= pairToAdd)
                            {
                                dodanie1 = pairToAdd;
                            }
                            else
                            {
                                dodanie1 = GetAnimalCardCount(animal1);
                            }
                        }
                    }
                    else
                    {
                        if (GetAnimalCardCount(animal1) >= pairToAdd + 1)
                        {
                            dodanie1 = pairToAdd + 1;
                        }
                        else
                        {
                            dodanie1 = GetAnimalCardCount(animal1);
                        }
                    }
                    player.AddToHerd(animal1, dodanie1);
                    DecreaseAnimalCardCount(animal1, dodanie1);
                }
                else
                {
                    if (animal1 == animal2)
                    {
                        player.AddToHerd(animal1, 1);
                        DecreaseAnimalCardCount(animal1, 1);

                    }
                }
                if (player.GetHerdCount(animal2) > 0)
                {
                    int animalcount2 = player.GetHerdCount(animal2);
                    int pairToAdd2 = animalcount2 / 2;
                    int dodanie2;
                    if (animal1 != animal2)
                    {
                        if (animalcount2 % 2 == 0)
                        {
                            if (GetAnimalCardCount(animal2) >= pairToAdd2)
                            {
                                dodanie2 = pairToAdd2;
                            }
                            else
                            {
                                dodanie2 = GetAnimalCardCount(animal2);
                            }
                        }
                        else
                        {
                            if (GetAnimalCardCount(animal2) >= pairToAdd2 + 1)
                            {
                                dodanie2 = pairToAdd2 + 1;
                            }
                            else
                            {
                                dodanie2 = GetAnimalCardCount(animal2);
                            }
                        }
                        player.AddToHerd(animal2, dodanie2);
                        DecreaseAnimalCardCount(animal2, dodanie2);
                    }
                }


            }
            catch (ErrorException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        public void StartGame()
        {
            Console.WriteLine("Welcome to Super Farmer!");

            Console.Write("Enter the number of players: ");
            int playerCount = int.Parse(Console.ReadLine());
            if (playerCount < 2 || playerCount > 4)
            {
                Console.WriteLine("Invalid number of players");
                return;
            }
            else
            {
                for (int i = 1; i <= playerCount; i++)
                {
                    Console.Write($"Enter the name of player {i}: ");
                    string playerName = Console.ReadLine();
                    players.Add(new Player(playerName));
                }
                bool gameWon = false;
                int round = 1;

                while (!gameWon)
                {
                    Console.WriteLine($"Round {round}");

                    foreach (Player player in players)
                    {
                        Console.WriteLine($"Player {player.Name}'s turn");
                        Console.WriteLine(player);
                        Console.WriteLine($"{player.Name}, choose an option:");
                        Console.WriteLine("1. Roll the dice");
                        Console.WriteLine("2. Trade animal cards");

                        int choiceBeforeRoll = int.Parse(Console.ReadLine());

                        switch (choiceBeforeRoll)
                        {
                            case 1:
                                var diceResult = dice.RollAnimalDice();
                                UpdateHerd(player, diceResult);
                                Console.WriteLine(player);
                                break;
                            case 2:

                                StartTrade(player);
                                break;
                            default:
                                Console.WriteLine("Invalid number. You lose your turn.");
                                break;
                        }

                        // Check for victory condition
                        if (player.HasAllRequiredAnimals())
                        {
                            Console.WriteLine($"{player.Name} wins!");
                            Console.WriteLine("Game Over. Results:");
                            EndGame();
                            gameWon = true;
                            break;
                        }
                    }
                    round++;
                }

            }
        }

        private void EndGame()
        {
            foreach (Player player in players)
            {
                Console.WriteLine(player);
                player.SaveSortedGameResultsToXml(players);
            }

        }

    }
}
