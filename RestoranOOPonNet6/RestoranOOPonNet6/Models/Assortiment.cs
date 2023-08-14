using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RestoranOOPonNet6.Models.Аssortiment;

namespace RestoranOOPonNet6.Models
{
    internal abstract class Аssortiment
    {
        public enum KindVariables
        {
            Patiekalas,
            Gėrimas
        }
        public Аssortiment() { }

        public Аssortiment(string name, double price)
        {

            Name = name;
            Price = price;
            double inputPrice = 0;            
        }
        public Аssortiment(string name, double price, string description) : this(name, price)
        {            
            Description = description;

        }

        public Аssortiment(KindVariables kindVariables, string name, double price, string description) : this(name, price, description)
        {
            Kind = kindVariables;
        }

        public Аssortiment CreateAssortiment()  //+unit
        {
            int inputChoice = 0;
            string inputName = "pavadinimas NENURODYTAS";
            double inputPrice = 0;
            string inputDescription = "papildomo aprašymo nėra";
            //var newDish = new Dish();
            
            do
            {
                Console.WriteLine("Jei kursi masitą - spausk 1, jei gėrimą - 2, nutraukti įvedimą - 0");
                inputChoice = Convert.ToInt16(Console.ReadLine());
            }
            while (inputChoice != 1 && inputChoice != 2);



            Console.WriteLine("Įvesk pavadinimą");
            inputName = Console.ReadLine();
            Console.WriteLine("Įvesk kaina");
            //inputPrice = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Įvesk aprašymą");            
            //inputDescription = Console.ReadLine();

            switch (inputChoice)
            {
                case 1:
                    Аssortiment createDish = new Dish(inputName, inputPrice, inputDescription);
                    //createDish.CreateDish(inputName, inputPrice, inputDescription);

                    if (UpdateNamesAndPrices(inputName, inputPrice))
                    { 
                    UpdateFullАssortiment(createDish);
                    return createDish;
                    }
                    else 
                    {
                        Console.WriteLine("OUUPS! UpdateNamesAndPrices sukurtas NULL dish......"); //for test only
                        return createDish = null;
                    } 

                default: Console.WriteLine("nenumatyta klaida");
                    return createDish = null;
            }

        }

        public bool UpdateNamesAndPrices(string name, double price) //neleisti pabaigti kūrimo, atnaujinimas neįvyks
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
        }

        public void UpdateFullАssortiment(Аssortiment assortiment ) 
        {
            FullАssortiment.Add(assortiment);            
            Console.WriteLine("UpdateFullАssortiment sėkminga");                
        }

        public void ShowFullАssortiment(KindVariables kindVariables) 
        {
            //KindVariables kindVariables
            Console.WriteLine("linq");
            var CurrentList = FullАssortiment.Where(k =>
            {
                //k.Kind == kindVariables;
                Console.WriteLine("pavadinimas: "+k.Name);
                return k.Kind == kindVariables;

            });
            Console.WriteLine("foreach");
            foreach (var current in FullАssortiment)
            { Console.WriteLine(current.Name); }

        }


        /*        public Аssortiment CreateDish(string inputName, double newPrice, string newDescription)
                {
                    var newDish = new Dish(KindVariables.Patiekalas, inputName, newPrice, newDescription);
                    return newDish;
                }*/

        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public KindVariables Kind { get; set; }
        public static List<Аssortiment> FullАssortiment { get; set; } = new List<Аssortiment>();
        public static Dictionary<string, double> NamesAndPrices { get; set; } = new Dictionary<string, double>();//name, price
    }
}

