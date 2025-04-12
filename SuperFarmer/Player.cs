using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace SuperFarmer
{

    /// <summary>
    /// Represents a player, which inherits from GameBox.
    /// Handles player's herd of animals, including adding/removing animals, trading, and updating herd information.
    /// Also supports saving sorted game results to an XML file and comparing players by name.
    /// </summary>
    
    public class Player : GameBox, IComparable<Player>
    {
        private string name;
        public List<Animal> herd;

        private string _herdInfoText;

        public string Name { get => name; set => name = value; }
        public List<Animal> Herd { get => herd; set => herd = value; }
        public event PropertyChangedEventHandler PropertyChanged;

        // Property to hold formatted herd information, triggers property change notification
        public string HerdInfoText
        {
            get { return _herdInfoText; }
            set
            {
                if (_herdInfoText != value)
                {
                    _herdInfoText = value;
                    OnPropertyChanged(nameof(HerdInfoText));
                }
            }
        }
        
        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public Player() : base()
        { 
            this.herd = new List<Animal>();
        }
        
        public Player(string name) : this()
        {
            this.name = name;
        }
        public void AddToHerd(EnumAnimal name, int count)
        {
            if (GetAnimalCardCount(name) > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    this.herd.Add(new Animal(name, count));       
                }
            }

        }

        //Winning condition
        public bool HasAllRequiredAnimals()
        {
            return GetHerdCount(EnumAnimal.Horse) >= 1 &&
                   GetHerdCount(EnumAnimal.Cow) >= 1 &&
                   GetHerdCount(EnumAnimal.Pig) >= 1 &&
                   GetHerdCount(EnumAnimal.Sheep) >= 1 &&
                   GetHerdCount(EnumAnimal.Rabbit) >= 1;
        }
        public int GetHerdCount(EnumAnimal animalType)
        {
            return Herd.Count(animal => animal.Name == animalType);
        }
        public void UpdateHerdInfo()
        {
           
            HerdInfoText = GetHerdAsString();
        }

        private string GetHerdAsString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in GetHerd())
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Gracz: {this.name}");
            sb.AppendLine("Aktualne stado: ");
            if (this.herd.Count == 0)
            {
                sb.AppendLine("Brak zwierząt");
            }
            else
            {
                foreach (EnumAnimal animalType in Enum.GetValues(typeof(EnumAnimal)))
                {
                    int animalCount = GetHerdCount(animalType);
                    if (animalCount > 0)
                    {
                        sb.AppendLine($"{animalType} ilość: {animalCount}");
                    }
                }
            }
            return sb.ToString();   
        }
        public Dictionary<EnumAnimal, int> GetHerd()
        {
            Dictionary<EnumAnimal, int> herdInfo = new Dictionary<EnumAnimal, int>();

            foreach (EnumAnimal animalType in Enum.GetValues(typeof(EnumAnimal)))
            {
                int animalCount = GetHerdCount(animalType);
                herdInfo.Add(animalType, animalCount);
            }

            return herdInfo;
        }

        public bool TradePossibility(EnumAnimal fromAnimal, EnumAnimal toAnimal, (int, int) ratio)
        {
            int availableFromAnimal = GetHerdCount(fromAnimal);
            int availableFromBox = GetAnimalCardCount(toAnimal);

            if (availableFromAnimal >= ratio.Item1 & availableFromBox >= ratio.Item2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TradeWithMainHerd(EnumAnimal fromAnimal, EnumAnimal toAnimal, (int, int) ratio)
        {
            int toAnimalCount = ratio.Item2;

            RemoveFromHerd(fromAnimal, ratio.Item1);
            AddToHerd(toAnimal, toAnimalCount);
  
        }

        public void RemoveFromHerd(EnumAnimal animal, int count)
        {
            int existingCount = GetHerdCount(animal);

            if (existingCount >= count)
            {
                
                for (int i = 0; i < count; i++)
                {
                    herd.Remove(herd.Find(a => a.Name == animal));
                }
            }
            else
            {
                Console.WriteLine($"Nie masz wystarczającej ilości {animal} do usunięcia ze stada.");
            }
        }

        public bool HasNoAnimals()
        {
            return herd.Count == 0;
        }

        public bool HasAnimal(EnumAnimal animal)
        {
            return herd.Any(a => a.Name == animal);
        }

        public void SaveSortedGameResultsToXml(List<Player> players)
        {
            
            players.Sort(); 

            string fileName = "sorted_wyniki_gry.xml";

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Player>));

                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(fileStream, Encoding.UTF8))
                    {
                        xmlWriter.Formatting = Formatting.Indented;

                        xmlWriter.WriteStartElement("Players");

                        // Iterate through players and save each one individually
                        foreach (Player player in players)
                        {
                            serializer.Serialize(xmlWriter, player);
                        }

                        xmlWriter.WriteEndElement();
                    }
                }

                Console.WriteLine($"Wyniki zapisane do posortowanego pliku XML: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas zapisywania posortowanych wyników gry do pliku XML: {ex.Message}");
            }
        }
    
        public int CompareTo(Player? other)
        {
            if (other == null)
                return 1; 

            // Compare players based on their names
            return string.Compare(this.Name, other.Name, StringComparison.Ordinal);
        }  
    }
}
    

