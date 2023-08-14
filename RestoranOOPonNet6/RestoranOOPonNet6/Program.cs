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

            /*            var dish = new Dish();
                        dish.CreateAssortiment();
                        dish.CreateAssortiment();

                        dish.ShowFullАssortiment(KindVariables.Patiekalas);*/

            TableAndPlace tableAndPlace = new TableAndPlace();
            tableAndPlace.CreateNewTable();
            tableAndPlace.ShowAllTablesInfo();

            }       
    }
}