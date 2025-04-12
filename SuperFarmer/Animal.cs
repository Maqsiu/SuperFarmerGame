using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SuperFarmer
{


    /// <summary>
    /// Contains the definition of the Animal class and EnumAnimal enumeration.
    /// The Animal class represents an animal with a specific type and quantity.
    /// The EnumAnimal enumeration lists all possible animal types in the game.
    /// </summary>
    
    public class Animal
    {
        private EnumAnimal name;
        private int count;

        public EnumAnimal Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }

        public Animal(EnumAnimal name, int count) 
        {
            this.name = name;
            this.count = count;
        }
    }

    public enum EnumAnimal
    {
        Rabbit,
        Sheep,
        Pig,
        Cow,
        Horse,
        Wolf,
        Fox,
        SmallDog,
        BigDog
    }
}
