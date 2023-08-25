using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal class Dish : Assortiment, ISimilarFuntions
    {

        public Dish CreateNewDish(string name, double price, string description)
        {
            var newDish = new Dish();
            AllDishes.Clear();
            ImportAllFromCSV();

            int newID = CheckMissedDishesID();
            if (newID == 0) newID = CheckForMaxUniqID()+1;
            Console.WriteLine("creating.........\n");

            newDish = new Dish(newID, name, price, description);
            AddDishToAllDishes(newDish);
            AddNewToCSV(newDish, "patiekalas");

            return newDish;
        }

        public int CheckMissedDishesID()
        {
            Console.WriteLine("\n\t CONTROL CheckMissedDishesID..........\n");

            int newID;
            if (FreeIDForNewDishes.Count > 0)
            {
                return newID = FreeIDForNewDishes[0];
            }
            else return newID = 0;
        }

        public int CheckForMaxUniqID()
        {
            Console.WriteLine("\n\t CONTROL CheckForMaxUniqID..........\n");
            int currentLargestUniqID = 0;
            if (AllDishes.Count > 0)
            {
                currentLargestUniqID = AllDishes.Max(d => d.UniqID);
                return currentLargestUniqID;
            }
            else
            {
                return currentLargestUniqID = 0;
            }
        }

        public void AddDishToAllDishes(Dish dish)
        {
            AllDishes.Add(dish);
        }

        public void NameChanger()
        {
            Console.WriteLine("\tPAVADINIMO KEITIMAS");
            SearchByName();
            InputNewName();
            //string temporaryAssortimentFilePath = Path.Combine(currentDirectory, "tempAssortiment.csv");
            //dishrightFilePath = Path.Combine(currentDirectory, "Dish.csv");
            SaveToTempAssortimentCSV("patiekalas");
            UpdateFile(dishFilePath, temporaryAssortimentFilePath);
        }

        public void DeleteDish()
        {
            Console.WriteLine("\tPATIEKALO ŠALINIMAS");
            SearchByName();
            DeleteByID();
            //string temporaryAssortimentFilePath = Path.Combine(currentDirectory, "tempAssortiment.csv");
            //dishrightFilePath = Path.Combine(currentDirectory, "Dish.csv");
            SaveToTempAssortimentCSV("patiekalas");
            UpdateFile(dishFilePath, temporaryAssortimentFilePath);
        }

        public void SearchByName()
        {
            Console.Write("Įveskite aktualaus patiekalo pavadinimą ar jo fragmentą: ");
            string partOfName = Console.ReadLine();
            var filteredDishes = AllDishes.Where(d => d.Name.Contains(partOfName)).ToList();
            if (filteredDishes.Count > 0)
                foreach (var dish in filteredDishes)
                {
                    Console.WriteLine($"\n{dish.UniqID} - {dish.Name} - {dish.Price}e ");
                }
            else
            {
                Console.WriteLine("\tAtitikimų nerasta\n");
            }
        }

        public void DeleteByID()
        {
            Console.Write("patiekalo ID? (0 - norint nutraukt veiksmą) ");
            int id = int.Parse(Console.ReadLine());
            if (id == 0) return;
            else
            {
                var selectedDish = AllDishes.FirstOrDefault(i => i.UniqID == id);
                AllDishes.Remove(selectedDish);
                FreeIDForNewDishes.Add(id);
            }
        }

        public Dish SelectByID()
        {
            Console.Write("patiekalo ID? ");
            int id = ConvertInputToIntIfPositive();
            if (id == 0)
            {
                BackToWelcome();
                return new Dish();
            }
            else
            {
                var selectedDish = AllDishes.FirstOrDefault(i => i.UniqID == id);
                if (selectedDish != null)
                {
                    return selectedDish;
                }
                else
                {
                    Console.WriteLine("nerastas gėrimas pagal įvestą iD");
                    BackToWelcome();
                    return new Dish();
                }
            }
        }

        public void InputNewName()
        {
            Console.Write("patiekalo ID? (0 - norint nutraukt veiksmą) ");
            int id = int.Parse(Console.ReadLine());
            if (id == 0) return;
            else
            {
                var selectedDish = AllDishes.FirstOrDefault(i => i.UniqID == id);
                Console.Write("naujas pavadinimas? ");
                string inputNewName = Console.ReadLine();
                selectedDish.Name = inputNewName;
            }
        }

        public static void RenewDishCsv()
        {



        }

        /*        public override string ConvertAssortimentToString(Dish dish)
                {
                    string dishToString = ($"{dish.Kind}, {dish.UniqID}, {dish.Name}, {dish.Description}");
                    return dishToString;
                }*/

        public static string DishNameDublicateControl()
        {
            string nameToCheck = Console.ReadLine();
            bool alreadyExist;
            do
            {
                alreadyExist = AllDishes.Any(a => a.Name == nameToCheck);
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
            //string dishFilePath = Path.Combine(currentDirectory, "Dishes.csv");
            if (IsFileAvailableToChange(dishFilePath))
            {
                using (StreamReader sr = new StreamReader(dishFilePath, Encoding.UTF8))
                {
                    string line;
                    int currentLine = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineValues = line.Split(';');
                        if (lineValues.Length == 6)
                        {
                            var recoveredDish = new Dish();

                            KindVariables kindVariables = KindVariables.Patiekalas;
                            int id = int.Parse(lineValues[1].Trim());
                            string name = lineValues[2].Trim().ToString();
                            double price = double.Parse(lineValues[3].Trim());
                            string description = lineValues[4].Trim().ToString();
                            DateTime creationDate = DateTime.Parse(lineValues[5].Trim());

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\tCONTROL nuskaitytos reiksmes -> {kindVariables}, {id}, {name}, {price}, {description}, {creationDate}");
                            Console.ResetColor();

                            recoveredDish = ConvertLineToDishFromFile(kindVariables, id, name, price, description, creationDate);

                            AddDishToAllDishes(recoveredDish);
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

        public Dish ConvertLineToDishFromFile(KindVariables kindVariables, int id, string name, double price, string description, DateTime date)
        {
            var dish = new Dish(kindVariables, id, name, price, description, date);
            return dish;
        }

        public void ClearAllLists() 
        {
            AllDishes.Clear();
            FreeIDForNewDishes.Clear();
        }

        /*        public void ImportAllFromCSV()
                {
                    throw new NotImplementedException();
                }*/

        /*        public static string ConvertObjectsToString(Аssortiment assortiment)
                {
                    //public Dish(string name, double price, string description) : base(name, price, description)
                    string assotrimentToString = ($"{assortiment.Name}, {assortiment.Price}, {assortiment.Description}, {assortiment.Kind},  ");
                    return
                }*/

        public void ShowAllDishes()
        {
            Console.WriteLine("\tCONTROL all dishes foreach........");
            foreach (var dish in AllDishes)
            {
                Console.WriteLine($"{dish.UniqID} {dish.Kind} \n{dish.Name} - {dish.Price} - {dish.Description} - {dish.CreationDate.ToString("yyyy-MM-dd")}");
            }
        }


        // savybes ir konstruktoriai
        public Dish() { }

        public Dish(int uniqID, string name, double price) : base(uniqID, name, price)
        {
            Kind = KindVariables.Patiekalas;
        }

        public Dish(int uniqID, string name, double price, string description) : base(uniqID, name, price, description)
        {
            Kind = KindVariables.Patiekalas;
        }

        public Dish(KindVariables kindVariables, int uniqID, string name, double price, string description, DateTime creationDate) : base(kindVariables, uniqID, name, price, description, creationDate)
        {
        }

        public static string dishFilePath = Path.Combine(currentDirectory, "Dishes.csv");

        public static List<Dish> AllDishes { get; set; } = new List<Dish>();
        public static List<int> FreeIDForNewDishes = new List<int>();
    }
}
