using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6.Models
{
    internal class Assortiment : CommonFunctions
    {

        public void CreateAssortiment()  //+unit
        {
            int inputChoice = 0;
            string inputName = "pavadinimas NENURODYTAS";
            double inputPrice = 0;
            string defaultDescription = "papildomo aprašymo nėra";
            string? repeatAction;
            //var newDish = new Dish();

            do
            {
                Console.WriteLine("Sukurti patiekalą - spausk 1, sukurti gėrimą - 2, nutraukti įvedimą - 0");
                inputChoice = Convert.ToInt16(Console.ReadLine());
                if (inputChoice == 0) break;
            }
            while (inputChoice != 1 && inputChoice != 2);

            do
            {
                Console.Write("Įvesk pavadinimą: ");
                if (inputChoice == 1)               
                inputName = Dish.DishNameDublicateControl();
                if (inputChoice == 2)
                    inputName = Drink.DrinkNameDublicateControl();
                if (string.IsNullOrEmpty(inputName)) return;

                Console.Write("Įvesk kaina: ");
                inputPrice = ConvertInputToDouble();
                if (inputPrice < 0) return;
                Console.Write("Įvesk aprašymą: ");
                string inputDescription = Console.ReadLine();
                if (string.IsNullOrEmpty(inputDescription)) inputDescription = defaultDescription;

                switch (inputChoice)
                {
                    case 1:
                        var dish = new Dish();
                        dish.CreateNewDish(inputName, inputPrice, inputDescription);
                        break;// createdDish;
                    case 2:
                        var drink = new Drink();
                        drink.CreateNewDrink(inputName, inputPrice, inputDescription);
                        break;// createdDrink;

                    default:
                        PrintSomethingWrong();
                        return; 
                }
                Console.Write("\nsukurti dar vieną? (+) ");
                repeatAction = Console.ReadLine();
            }
            while (repeatAction == "+");
        }

        /*        public bool UpdateNamesAndPrices(string name, double price) //neleisti pabaigti kūrimo, atnaujinimas neįvyks
                {
                    if (NamesAndPrices.TryAdd(name, price))
                    {
                        Console.WriteLine($"{name} kaina {price} - išsaugota");
                        return true;
                    }
                    else 
                    {
                        Console.WriteLine("UpdateNamesAndPrices klaida (tikėtina toks patiekalas/gėrimas jau egzistuoja)");
                        return false;            
                    }
                }*/
        /*
                public void UpdateFullАssortiment(Assortiment assortiment ) 
                {
                    FullАssortiment.Add(assortiment);            
                    Console.WriteLine("UpdateFullАssortiment sėkminga");                
                }*/

        public void SaveToTempAssortimentCSV(string dishOrDrink)
        {
            //string temporaryAssortimentFilePath = Path.Combine(currentDirectory, "tempAssortiment.csv");
            if (dishOrDrink == "patiekalas")
            {
                using (StreamWriter sw = new StreamWriter(temporaryAssortimentFilePath))
                {
                    Console.WriteLine("\tall dishes foreach for renewing........");
                    foreach (var dish in Dish.AllDishes)
                    {
                     string line = ConvertAssortimentToString(dish);
                     sw.WriteLine(line); 
                    }
                }
            }
            else if (dishOrDrink == "gerimas")
            {
                using (StreamWriter sw = new StreamWriter(temporaryAssortimentFilePath))
                {
                    Console.WriteLine("\tall dishes foreach for renewing........");
                    foreach (var drink in Drink.AllDrinks)
                    {
                        string line = ConvertAssortimentToString(drink);
                        sw.WriteLine(line);
                    }
                }
            }
            else Console.WriteLine("SaveToTempAssortimentCSV oups");
        }


        public void AddNewToCSV(Assortiment assortiment, string dishOrDrink)
        {
            string targetFilePath = ChooseRightAssortimentFile(dishOrDrink);
            using (StreamWriter sw = new StreamWriter(targetFilePath, true, Encoding.UTF8))
            {
                string line = ConvertAssortimentToString(assortiment);
                sw.WriteLine(line);
            }
        }


/*        public static void AddNewToTempCSV(Dish dish)
        {
            //string tableFilePath = currentDirectory + "Tables.csv";
            string dishFilePath = Path.Combine(currentDirectory, "Dishes.csv");
            using (StreamWriter sw = new StreamWriter(dishFilePath, true, Encoding.UTF8))
            {
                string line = ConvertAssortimentToString(dish);
                sw.WriteLine(line);
            }
        }*/


        public string ConvertAssortimentToString(Assortiment assortiment)
        {
            {
                string dishToString = ($"{assortiment.Kind}; {assortiment.UniqID}; {assortiment.Name}; {assortiment.Price}; {assortiment.Description}; {assortiment.CreationDate}");
                return dishToString;
            }
        }


/*        public void ShowАssortiment(int actualKind)
        {

            if (actualKind == 1)
            {
                var dish = new Dish();
                dish.ShowAllDishes();
            }
            else PrintSomethingWrong();
        }
*/

        /*        public Аssortiment CreateDish(string inputName, double newPrice, string newDescription)
                {
                    var newDish = new Dish(KindVariables.Patiekalas, inputName, newPrice, newDescription);
                    return newDish;
                }*/

        public static string ChooseRightAssortimentFile(string dishOrDrink) 
        {
            string rightFilePath;
            if (dishOrDrink == "patiekalas")
                return rightFilePath = Path.Combine(currentDirectory, "Dishes.csv");
            else if (dishOrDrink == "gerimas") return rightFilePath = Path.Combine(currentDirectory, "Drinks.csv");
            else
            {                
                Console.WriteLine("neegzistuojantis asoritmentas");
                return rightFilePath = string.Empty;            
            }
        }

        // savybes ir konstruktoriai
        public enum KindVariables
        {
            Patiekalas,
            Gėrimas
        }
        public Assortiment() { }

        public Assortiment(int uniqID, string name, double price)
        {
            UniqID = uniqID;
            Name = name;
            Price = price;
            CreationDate = DateTime.Today;
        }
        public Assortiment(int uniqID, string name, double price, string description) : this(uniqID, name, price)
        {
            Description = description;

        }

        public Assortiment(KindVariables kindVariables, int uniqID, string name, double price, string description, DateTime creationDate)
        {
            Kind = kindVariables;
            UniqID = uniqID;
            Name = name;
            Price = price;
            Description = description;
            CreationDate = creationDate;
        }


        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public DateTime CreationDate { get; set; } = new DateTime();
        public KindVariables Kind { get; set; }
        public int UniqID { get; set; }

        public static string temporaryAssortimentFilePath = Path.Combine(currentDirectory, "tempAssortiment.csv");
        //public static List<Assortiment> FullАssortiment { get; set; } = new List<Assortiment>();
        //public static Dictionary<string, double> NamesAndPrices { get; set; } = new Dictionary<string, double>();//name, price
    }
}

