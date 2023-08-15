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
            tableAndPlace.CreateNewTable();
            tableAndPlace.ShowAllTablesInfo();

        }
    }
}