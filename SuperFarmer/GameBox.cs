using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFarmer
{

    /// <summary>
    /// Represents the game box in SuperFarmer, which contains animal cards used during gameplay.
    /// Provides methods to access, modify, and display the quantity of each animal type.
    /// Handles logic for adding and removing animal cards, as well as error handling when cards are unavailable.
    /// </summary>
    public class GameBox 
    {
        private Dictionary<EnumAnimal, int> animalCards;
        public Dictionary<EnumAnimal, int> AnimalCards { get => animalCards; set => animalCards = value; }
        
        public GameBox() 
        {

            this.animalCards = new Dictionary<EnumAnimal, int>
            {
                { EnumAnimal.Rabbit, 60 },
                { EnumAnimal.Sheep, 24 },
                { EnumAnimal.Pig, 20 },
                { EnumAnimal.Cow, 12 },
                { EnumAnimal.Horse, 6 },
                { EnumAnimal.SmallDog, 4 },
                { EnumAnimal.BigDog, 2 }};
        }


        public int GetAnimalCardCount(EnumAnimal animal)
        {
            if (animalCards.ContainsKey(animal))
            {
                return animalCards[animal];
            }
            else
            {
                throw new ErrorException($"No more {animal} in the game box.");

            }
        }

        public Dictionary<EnumAnimal, int> GetAnimalCards()
        {
            return this.animalCards;
        }

        public void PrintAnimalCardCounts()
        {
            foreach (var kvp in animalCards)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value} cards");
            }
        }
       

        public void DecreaseAnimalCardCount(EnumAnimal animal, int count)
        {

            if (animalCards.ContainsKey(animal))
            {
                if (animalCards[animal] >= count)
                {
                    animalCards[animal] -= count;
                }
                else
                {
                    animalCards[animal] = 0;
                    throw new ErrorException($"No more {animal} in the game box. You do not receive additional cards.");
                }
                gameBoxContents();
            }
            else
            {
                throw new ErrorException($"The species {animal} is not available in the game box.");
            }



        }

        public void gameBoxContents()
        {
            Console.WriteLine("Contents of the game box:");

            foreach (var kvp in this.animalCards)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
        public void IncreaseAnimalCardCount(EnumAnimal animal, int count)
        {
            
            if (animalCards.ContainsKey(animal))
            {
                animalCards[animal] += count;
            }
            else
            {
                animalCards[animal] = count;
            }
            gameBoxContents(); //added
        }
    }
}
