﻿/*using RestoranOOPonNet6.Models;
using System.Reflection;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6
{
    internal class Program
    {
            static void Main(string[] args)
            {
                Console.WriteLine("Hello again my restaurant!!!\n");

            //string currentDirectory = Directory.GetCurrentDirectory()+"\\myFiles";
            //Console.WriteLine("Esamas folderio kelias: " + currentDirectory);

            //Console.WriteLine(DateTime.Now);

            //(int table, int orderID, string itemName, double itemPrice, int itemQ)


            // asortimento valdymas
            var assortiment = new Assortiment();
                        var dish = new Dish();
                        dish.ImportAllFromCSV();

            *//*            //dishes
                        //assortiment.CreateAssortiment();
                        //dish.NameChanger();
                        dish.ShowAllDishes();

                        dish.DeleteDish();
                        dish.ImportAllFromCSV();
                        dish.ShowAllDishes();
                        //dishes end*/

/*            //drinks
            var drink = new Drink();
            drink.ImportAllFromCSV();
            assortiment.CreateAssortiment();
            //drink.NameChanger();
            drink.ShowAllDrinks();

            drink.DeleteDish();
            drink.ImportAllFromCSV();
            drink.ShowAllDrinks();
            //drinks end *//*




            // stalu valdymas
            #region
            TableAndPlace tableAndPlace = new TableAndPlace();
            *//*            string inputAvailibleStatus;
                        string inputTableStatus = Console.ReadLine();
                        if (inputTableStatus == "1") inputAvailibleStatus = "True";
                        else if (inputTableStatus == "2") inputAvailibleStatus = "False";
                        else inputAvailibleStatus = string.Empty;
                        Console.WriteLine(inputAvailibleStatus);*//*

            tableAndPlace.ImportAllFromCSV();
            //TableAndPlace.FilterTablesByFreePlaces(3);
            //tableAndPlace.CreateNewTable();
            //tableAndPlace.RemoveTableFromCSV(0);
            //tableAndPlace.OcupideTableEverywere(1, 1);
            //tableAndPlace.DeOcupideTableEverywere(1);
            //
            //tableAndPlace.ShowTableInfo(TableAndPlace.AllTabels);
            //tableAndPlace.SelectActualTablesFromCSV();
            *//*            string t = "False";
                        Console.WriteLine("enter or True");
                        string inputAvailibleStatus = Console.ReadLine();
                        bool isAvailable = string.IsNullOrEmpty(inputAvailibleStatus) ? bool.Parse(t) : bool.Parse(inputAvailibleStatus);
                        Console.WriteLine(isAvailable);*//*

            //tableAndPlace.SelectByTableNrFromFile("4");
            #endregion
            //stalu valdymas END

            // order item valdymas
            var oc = new OrderContent();
            oc.ImportAllFromCSV();

            *//*            oc.CreateActiveOrderContent(1, 2, "kaku", 2, 5);
                        oc.CreateActiveOrderContent(2, 3, "kaku", 2, 5);*//*
                        oc.CloseActiveOrderContent();
            *//*          // order item valdymas END

            //order summary
            var order = new OrderSummary();
            order.ImportAllFromCSV();
            //order.CreateOrder(); //reserve table...
            //order.CloseActiveOrder();
            //order.InputPredataForNewOrder();
            //order.InputDishOrDrink();
            order.AddItemToOrder();
        }

    }
}*/