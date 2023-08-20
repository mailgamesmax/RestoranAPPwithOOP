using RestoranOOPonNet6.Models;
using System.Reflection;
using static RestoranOOPonNet6.Models.Аssortiment;

namespace RestoranOOPonNet6
{
    internal class Program
    {
            static void Main(string[] args)
            {
                Console.WriteLine("Hello again my restaurant!!! ;\n");

             string currentDirectory = Directory.GetCurrentDirectory()+"\\myFiles";
            //Console.WriteLine("Esamas folderio kelias: " + currentDirectory);

            

            // asortimento kurimo testas
            /*            var dish = new Dish();
                        dish.CreateAssortiment();
                        dish.CreateAssortiment();

                        dish.ShowFullАssortiment(KindVariables.Patiekalas);*/


            // stalu kurimo testas
            TableAndPlace tableAndPlace = new TableAndPlace();
            /*            string inputAvailibleStatus;
                        string inputTableStatus = Console.ReadLine();
                        if (inputTableStatus == "1") inputAvailibleStatus = "True";
                        else if (inputTableStatus == "2") inputAvailibleStatus = "False";
                        else inputAvailibleStatus = string.Empty;
                        Console.WriteLine(inputAvailibleStatus);*/

            tableAndPlace.ImportAllFromCSV();
            //TableAndPlace.FilterTablesByFreePlaces(3);
            //tableAndPlace.CreateNewTable();
            //tableAndPlace.RemoveTableFromCSV(0);
            //tableAndPlace.OcupideTableInCSV(2);
            tableAndPlace.ShowTableInfo(TableAndPlace.AllTabels);
            //tableAndPlace.SelectActualTablesFromCSV();
            /*            string t = "False";
                        Console.WriteLine("enter or True");
                        string inputAvailibleStatus = Console.ReadLine();
                        bool isAvailable = string.IsNullOrEmpty(inputAvailibleStatus) ? bool.Parse(t) : bool.Parse(inputAvailibleStatus);
                        Console.WriteLine(isAvailable);*/

            //tableAndPlace.SelectByTableNrFromFile("4");
        }
    }
}