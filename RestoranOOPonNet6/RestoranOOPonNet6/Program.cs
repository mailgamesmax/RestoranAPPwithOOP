using RestoranOOPonNet6.Models;
using System.Reflection;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6
{
    internal class Program
    {
            static void Main(string[] args)
            {
                Console.WriteLine("Hello again my restaurant!!! ;\n");

            //string currentDirectory = Directory.GetCurrentDirectory()+"\\myFiles";
            //Console.WriteLine("Esamas folderio kelias: " + currentDirectory);



            // asortimento valdymas
            var assortiment = new Assortiment();

            /*            //dishes
                        var dish = new Dish();
                        dish.ImportAllFromCSV();
                        //assortiment.CreateAssortiment();
                        //dish.NameChanger();
                        dish.ShowAllDishes();

                        dish.DeleteDish();
                        dish.ImportAllFromCSV();
                        dish.ShowAllDishes();
                        //dishes end*/

            //drinks
            var drink = new Drink();
            drink.ImportAllFromCSV();
            assortiment.CreateAssortiment();
            drink.NameChanger();
            drink.ShowAllDrinks();

/*            drink.DeleteDish();
            drink.ImportAllFromCSV();
            drink.ShowAllDrinks();*/
            //drinks end




            // stalu valdymas
            #region
            TableAndPlace tableAndPlace = new TableAndPlace();
            /*            string inputAvailibleStatus;
                        string inputTableStatus = Console.ReadLine();
                        if (inputTableStatus == "1") inputAvailibleStatus = "True";
                        else if (inputTableStatus == "2") inputAvailibleStatus = "False";
                        else inputAvailibleStatus = string.Empty;
                        Console.WriteLine(inputAvailibleStatus);*/

            //tableAndPlace.ImportAllFromCSV();
            //TableAndPlace.FilterTablesByFreePlaces(3);
            //tableAndPlace.CreateNewTable();
            //tableAndPlace.RemoveTableFromCSV(0);
            //tableAndPlace.OcupideTableEverywere(1, 1);
            //tableAndPlace.DeOcupideTableEverywere(1);
            //
            //tableAndPlace.ShowTableInfo(TableAndPlace.AllTabels);
            //tableAndPlace.SelectActualTablesFromCSV();
            /*            string t = "False";
                        Console.WriteLine("enter or True");
                        string inputAvailibleStatus = Console.ReadLine();
                        bool isAvailable = string.IsNullOrEmpty(inputAvailibleStatus) ? bool.Parse(t) : bool.Parse(inputAvailibleStatus);
                        Console.WriteLine(isAvailable);*/

            //tableAndPlace.SelectByTableNrFromFile("4");
            #endregion
            //stalu valdymas END



        }
    }
}