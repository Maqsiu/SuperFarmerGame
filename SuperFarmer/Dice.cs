using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFarmer
{

    /// <summary>
    /// Represents the dice roll. 
    /// Simulates rolling two 12-sided dice and returns animal types based on the result.
    /// Includes error handling for invalid dice results.
    /// </summary>
    
    public class Dice : GameBox
    {
        private Random random;

        public Dice() 
        {
            random = new Random();
        }

        public (EnumAnimal, EnumAnimal) RollAnimalDice()
        {
            // Symulacja rzutu dwiema kostkami 12-ściennymi
            int result1 = random.Next(1, 13);
            int result2 = random.Next(1, 13);

            try
            {
                EnumAnimal animalType1 = GetAnimalType1(result1);
                EnumAnimal animalType2 = GetAnimalType2(result2);

                Console.WriteLine($"Roll 1: {result1}, Animal Type 1: {animalType1}");
                Console.WriteLine($"Roll 2: {result2}, Animal Type 2: {animalType2}");
                return (animalType1, animalType2);
            }
            catch (ErrorException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (EnumAnimal.Wolf, EnumAnimal.Fox);
            }
        }

        public EnumAnimal GetAnimalType1(int result1)
        {
            switch (result1)
            {
                case int n when (n <= 6):
                    return EnumAnimal.Rabbit;
                case int n when (n <= 9):
                    return EnumAnimal.Sheep;
                case 10:
                    return EnumAnimal.Pig;
                case 11:
                    return EnumAnimal.Cow;
                case 12:
                    return EnumAnimal.Wolf;
                default:
                    throw new ErrorException("Error during dice roll");
            }
        }
        public EnumAnimal GetAnimalType2(int result2)
        {
            switch (result2)
            {
                case int n when (n <= 6):
                    return EnumAnimal.Rabbit;
                case int n when (n <= 9):
                    return EnumAnimal.Sheep;
                case 10:
                    return EnumAnimal.Pig;
                case 11:
                    return EnumAnimal.Horse;
                case 12:
                    return EnumAnimal.Fox;
                default:
                    throw new ErrorException("Error during dice roll");
            }
        }
    }

    // Custom exception used for errors during dice operations
    public class ErrorException : Exception
    {
        public ErrorException(string message) : base(message) { }
    }   
}
