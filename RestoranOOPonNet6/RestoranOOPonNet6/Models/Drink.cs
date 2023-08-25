using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal class Drink : Assortiment, ISimilarFuntions
    {

        public Drink CreateNewDrink(string name, double price, string description)
        {
            var newDrink = new Drink();
            AllDrinks.Clear();
            ImportAllFromCSV();

            int newID = CheckMissedDrinksID();
            if (newID == 0) newID = CheckForMaxUniqID() + 1;
            Console.WriteLine("creating.........\n");

            newDrink = new Drink(newID, name, price, description);
            AddDrinkToAllDrinks(newDrink);
            AddNewToCSV(newDrink, "gerimas");

            return newDrink;
        }

        public int CheckMissedDrinksID()
        {
            Console.WriteLine("\n\t CONTROL CheckMissedDishesID..........\n");

            int newID;
            if (FreeIDForNewDrinks.Count > 0)
            {
                return newID = FreeIDForNewDrinks[0];
            }
            else return newID = 0;
        }

        public int CheckForMaxUniqID()
        {
            Console.WriteLine("\n\t CONTROL CheckForMaxUniqID..........\n");
            int currentLargestUniqID = 0;
            if (AllDrinks.Count > 0)
            {
                currentLargestUniqID = AllDrinks.Max(d => d.UniqID);
                return currentLargestUniqID;
            }
            else
            {
                return currentLargestUniqID = 0;
            }
        }

        public void AddDrinkToAllDrinks(Drink dish)
        {
            AllDrinks.Add(dish);
        }

        public void NameChanger()
        {
            Console.WriteLine("\tPAVADINIMO KEITIMAS");
            SearchByName();
            InputNewName();
            //string temporaryAssortimentFilePath = Path.Combine(currentDirectory, "tempAssortiment.csv");
            //drinkRightFilePath = Path.Combine(currentDirectory, "Drink.csv");
            SaveToTempAssortimentCSV("gerimas");
            UpdateFile(drinkFilePath, temporaryAssortimentFilePath);
        }

        public void DeleteDish()
        {
            Console.WriteLine("\tGĖRIMO ŠALINIMAS");
            SearchByName();
            DeleteByID();
            SaveToTempAssortimentCSV("gerimas");
            UpdateFile(drinkFilePath, temporaryAssortimentFilePath);
        }

        public void SearchByName()
        {
            Console.Write("Įveskite aktualaus patiekalo pavadinimą ar jo fragmentą: ");
            string partOfName = Console.ReadLine();
            var filteredDishes = AllDrinks.Where(d => d.Name.Contains(partOfName)).ToList();
            foreach (var dish in filteredDishes)
            {
                Console.WriteLine($"{dish.UniqID} - {dish.Name} - {dish.Price}e ");
            }
        }

        public void DeleteByID()
        {
            Console.Write("gėrimo ID? (0 - norint nutraukt veiksmą) ");
            int id = int.Parse(Console.ReadLine());
            if (id == 0) return;
            else
            {
                var changeThisDish = AllDrinks.FirstOrDefault(i => i.UniqID == id);
                AllDrinks.Remove(changeThisDish);
                FreeIDForNewDrinks.Add(id);
            }
        }

        public Drink SelectByID()
        {
            Console.Write("gėeimo ID? (0 - norint nutraukt veiksmą) ");
            int id = int.Parse(Console.ReadLine());
            if (id == 0) return new Drink();
            else
            {
                var selectedDrink = AllDrinks.FirstOrDefault(i => i.UniqID == id);
                return selectedDrink;
            }
        }

        public void InputNewName()
        {
            Console.Write("gėrimo ID? (0 - norint nutraukt veiksmą) ");
            int id = int.Parse(Console.ReadLine());
            if (id == 0) return;
            else
            {
                var changeThisDrink = AllDrinks.FirstOrDefault(i => i.UniqID == id);
                Console.Write("naujas pavadinimas? ");
                string inputNewName = Console.ReadLine();
                changeThisDrink.Name = inputNewName;
            }
        }

        public static void RenewDishCsv()
        {



        }

        public static string DrinkNameDublicateControl()
        {
            string nameToCheck = Console.ReadLine();
            bool alreadyExist;
            do
            {
                alreadyExist = AllDrinks.Any(a => a.Name == nameToCheck);
                if (alreadyExist)
                {
                    Console.Write($"{nameToCheck} jau egzistuoja. Sukurkite kitą pavadinimą (enter - nutraukti operaciją): ");
                    nameToCheck = Console.ReadLine();
                    if (string.IsNullOrEmpty(nameToCheck)) return nameToCheck;
                }

            }
            while (alreadyExist);

            return nameToCheck;
        }


        public void ImportAllFromCSV()
        {
            if (IsFileAvailableToChange(drinkFilePath))
            {
                using (StreamReader sr = new StreamReader(drinkFilePath, Encoding.UTF8))
                {
                    string line;
                    int currentLine = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineValues = line.Split(';');
                        if (lineValues.Length == 6)
                        {
                            var recoveredDish = new Drink();

                            KindVariables kindVariables = KindVariables.Gėrimas;
                            int id = int.Parse(lineValues[1].Trim());
                            string name = lineValues[2].Trim().ToString();
                            double price = double.Parse(lineValues[3].Trim());
                            string description = lineValues[4].Trim().ToString();
                            DateTime creationDate = DateTime.Parse(lineValues[5].Trim());

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\tnuskaitytos reiksmes -> {kindVariables}, {id}, {name}, {price}, {description}, {creationDate}");
                            Console.ResetColor();

                            recoveredDish = ConvertLineToDrinkFromFile(kindVariables, id, name, price, description, creationDate);

                            AddDrinkToAllDrinks(recoveredDish);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{currentLine} eilutėje klaidingas  savybių kiekis");
                            Console.ResetColor();
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("\t<<< nenumatyta duomenų failo problema >>>\n");
                return;
            }
        }

        public Drink ConvertLineToDrinkFromFile(KindVariables kindVariables, int id, string name, double price, string description, DateTime date)
        {
            var dish = new Drink(kindVariables, id, name, price, description, date);
            return dish;
        }


        public void ShowAllDrinks()
        {
            Console.WriteLine("all dishes foreach........");
            foreach (var dish in AllDrinks)
            {
                Console.WriteLine($"{dish.UniqID} {dish.Kind} \n{dish.Name} - {dish.Price} - {dish.Description} - {dish.CreationDate.ToString("yyyy-MM-dd")}");
            }
        }


        // savybes ir konstruktoriai
        public Drink() { }

        public Drink(int uniqID, string name, double price) : base(uniqID, name, price)
        {
            Kind = KindVariables.Gėrimas;
        }

        public Drink(int uniqID, string name, double price, string description) : base(uniqID, name, price, description)
        {
            Kind = KindVariables.Gėrimas;
        }

        public Drink(KindVariables kindVariables, int uniqID, string name, double price, string description, DateTime creationDate) : base(kindVariables, uniqID, name, price, description, creationDate)
        {
        }

        public static string drinkFilePath = Path.Combine(currentDirectory, "Drinks.csv");

        public static List<Drink> AllDrinks { get; set; } = new List<Drink>();
        public static List<int> FreeIDForNewDrinks = new List<int>();
    }
}

