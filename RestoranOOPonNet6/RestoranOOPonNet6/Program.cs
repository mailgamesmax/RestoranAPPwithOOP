using RestoranOOPonNet6.Models;
using System;
using System.Reflection;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6
{
    internal class Program
    {
            static void Main(string[] args)
            {
                Console.WriteLine("Hello again my restaurant!!!\n");

            var assortiment = new Assortiment();
            var dish = new Dish();
            var drink = new Drink();
            var table = new TableAndPlace();
            var order = new OrderSummary();
            var oc = new OrderContent();
            
            string timeToExit = "n";

            do
            {
                ResetListsAndImportEverything();

                Console.WriteLine("viskas prasideda čia! \n");

                // staliuku valdymo meniu
                int? choosedAction = null;
                GoToActionOptions(ref choosedAction);
                switch (choosedAction)
                {
                    case 11:                        
                        table.ShowAllTablesInfo();                        
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                    case 12:
                        table.FilterActualTablesFromCSV();
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                    case 13:
                        Console.Write("Kurį staliuką norite patikrinti?");
                        string checkTableNr = Console.ReadLine();
                        table.ShowByTableNrFromFile(checkTableNr);
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                    case 14:
                        table.CreateNewTable();
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                    case 21:
                        order.CreateOrder();
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                    case 22:
                        order.AddItemToOrder();
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                    case 23:
                        order.CloseActiveOrder();
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                    case 31:
                        dish.SearchByName();
                        Console.WriteLine();
                        break;
                    case 32:
                        drink.SearchByName();                        
                        Console.WriteLine();
                        break;
                    case 33:
                        dish.ShowAllDishes();
                        Console.WriteLine();
                        break;
                    case 34:
                        drink.ShowAllDrinks();
                        Console.WriteLine();
                        break;
                    case 35:
                        assortiment.CreateAssortiment();
                        Console.WriteLine();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("įvestas dar nesukurtas funkcionalumas )) ");
                        Environment.Exit(0);
                        Console.WriteLine();
                        //BackToMeniuOrExitButton();
                        break;
                }

                //                goto StartingStep;

                //countNamesAndKindes.Add("Šerbet", "pavadinimas");
                //dishesFullInfo.Add(namesAndKindes);
                //AvailableTables(availableTablesPlaces);


                //CheckTableStatus(availableTablesPlaces, reservedTablesPlaces, reserveTable);
                //CheckTablesOrders(tablesBills, reserveTable);

/*                Console.Write("Time to exit?? (y)");
                timeToExit = Console.ReadLine().ToLower();
*/
            //                StartingStep:

/*            StartingStep:
                Console.Write("Išvalyt ekraną? (y)");
                string clearDisplay = Console.ReadLine().ToLower();
                if (clearDisplay == "y")
                {
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                }*/

            } //main do uzdarymas
            //while (timeToExit != "y");
            while (true);

            //            GoToActionOptions(ref choosedAction2);

        }

        //// METODAI
        //
        //start program

        public static void ResetListsAndImportEverything()
        {
            var assortiment = new Assortiment();
            var dish = new Dish();
            var drink = new Drink();
            var table = new TableAndPlace();
            var order = new OrderSummary();
            var oc = new OrderContent();

            dish.ClearAllLists();
            drink.ClearAllLists();
            table.ClearAllLists();
            order.ClearAllLists();
            oc.ClearAllLists();

            dish.ImportAllFromCSV();
            drink.ImportAllFromCSV();
            table.ImportAllFromCSV();
            order.ImportAllFromCSV();
            oc.ImportAllFromCSV();
        }

        public static void GoToActionOptions(ref int? choosedAction)
        {
            do
            {
                //Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Pasirinkite veiksmą: \n:)\n" +
                "STALŲ VALDYMAS\n 11 - Parodyti visus stalus ir jų vietas \n 12 - Parodyti staliukus pagal aktualius kriterijus \n 13 - patikrinti staliuko būseną \n 14 – Pridėti naują stalą \nUŽSAKYMŲ VALDYMAS \n 21 - Rezervuoti/Užimti stalą \n 22 – Papildyti stalo užsakymą 23 - Patvirtinti APMOKĖJIMĄ ir atlaisvinti stalą \nASORTIMENTO VALDYMAS \n 31 – Rasti patiekalą \n 32 – Rasti gėrimą \n 33 – parodyti visus patiekalus \n 34 – parodyti visus gėrimus Parodyti visus patiekalus \n 35 – Pridėti naują patiekalą ar gėrimą \n 0 - Uždaryti programą");
                //bool userInput = int.TryParse(input, out var number);
                bool userInput = int.TryParse(Console.ReadLine(), out int userNumber);
                choosedAction = userNumber;
                Console.ResetColor();
                Console.WriteLine();
                //if (choosedAction != 1 && choosedAction != 2 && choosedAction != 3);

                if (userInput == false || choosedAction < 0 || choosedAction > 40)
                {
                    Console.WriteLine("Įvedimo klaida. Grįžti (spausti betkokį klavišą).");
                    Console.ReadKey();
                    Console.Clear();
                }

            }
            //while (choosedAction != 1 && choosedAction != 2 && choosedAction != 3);
            while (choosedAction < 0 || choosedAction > 40);

        } // end GToActionOptions(choosedAction)

    }
}
