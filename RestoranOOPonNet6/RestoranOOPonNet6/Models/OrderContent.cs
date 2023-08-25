using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6.Models
{
    internal class OrderContent : CommonFunctions, ISimilarFuntions
    {
        //public double CreateActiveOrderContent(int table, string orderID, string itemName, double itemPrice, double itemQ)
        public double CreateActiveOrderContent(int table, string orderID)
        {
            var newOrderContent = new OrderContent(table, orderID);
            AddNewToCSV(newOrderContent);
            AllActiveOrdersContent.Add(newOrderContent);
            return newOrderContent.LinePrice;
        }

        public double AddIDishItemToOrder(int table, string orderID)
        {
            var dish = new Dish();
            var addDish = dish.SelectByID();
            if (addDish == null) return 0; // nepritaikyta klaidingai ivedus neegzistuojanti ID
            Console.Write("Kiekis? ");
            double quantity = ConvertInputToDoubleIfPositive();

            string name = addDish.Name.ToString();
            double price = Convert.ToDouble(addDish.Price);

            var newOrderContent = new OrderContent(table, orderID, name, price, quantity);
            AddNewToCSV(newOrderContent);
            AllActiveOrdersContent.Add(newOrderContent);
            return newOrderContent.LinePrice;
        }

        public double AddIDrinkItemToOrder(int table, string orderID)
        {
            var drink = new Drink();
            var addDrink = drink.SelectByID();
            if (addDrink == null) return 0; // nepritaikyta klaidingai ivedus neegzistuojanti ID
            Console.Write("Kiekis? ");
            double quantity = ConvertInputToDoubleIfPositive();
            string name = addDrink.Name.ToString();
            double price = Convert.ToDouble(addDrink.Price);

            var newOrderContent = new OrderContent(table, orderID, name, price, quantity);
            AddNewToCSV(newOrderContent);
            AllActiveOrdersContent.Add(newOrderContent);
            return newOrderContent.LinePrice;
        }

        public void AddNewToCSV(OrderContent newOrderContent)
        {
            using (StreamWriter sw = new StreamWriter(ActiveOrdersContentsFilePath, true, Encoding.UTF8))
            {
                string line = ConvertOrderContentToString(newOrderContent);
                sw.WriteLine(line);
            }
        }

        public string ConvertOrderContentToString(OrderContent newOrderContent)
        {
            {
                string orderContentToString = $"{newOrderContent.Table}; {newOrderContent.OrderID}; {newOrderContent.ItemName}; {newOrderContent.ItemPrice}; {newOrderContent.ItemQ}; {newOrderContent.LinePrice}";
                return orderContentToString;
            }
        }

        public OrderContent ConvertLineToOrderContent(int table, string orderID, string itemName, double itemPrice, double itemQ, double linePrice)
        {
            var orderContent = new OrderContent(table, orderID, itemName, itemPrice, itemQ, linePrice);
            return orderContent;
        }



        public List<OrderContent> SelectActiveByTable(int tableNr)
        {
            if (tableNr < 1) return null;
            else
            {
                var allItemsOfSelectedActiveOrder = AllActiveOrdersContent.Where(c => c.Table == tableNr).ToList();

                foreach (var item in allItemsOfSelectedActiveOrder) // CONTROL
                {
                    Console.WriteLine($"selected table {item.Table}");
                }
                return allItemsOfSelectedActiveOrder; //paskelbti isnanksto?
            }
        }

        public int CloseActiveOrderContent()
        {
            Console.Write("Užsakymo stalo nr? (0 - norint nutraukt veiksmą) ");
            int tableNr = int.Parse(Console.ReadLine());

            List<OrderContent> allItemsOfSelectedActiveOrder = new List<OrderContent>();
            allItemsOfSelectedActiveOrder = SelectActiveByTable(tableNr);

            Console.WriteLine("\tCONTROL Close listo papildymas.........\n");
            if (allItemsOfSelectedActiveOrder != null)
            {
                AllClosedOrdersContent.AddRange(allItemsOfSelectedActiveOrder);

                foreach (var item in AllClosedOrdersContent) // CONTROL
                {
                    Console.WriteLine($"CONTROL prideta closed tables {item.Table}");
                }


                Console.WriteLine("\tCONTROL OPEN šalinimas iš listo .........\n");
                AllActiveOrdersContent.RemoveAll(item => allItemsOfSelectedActiveOrder.Contains(item));

                foreach (var item in allItemsOfSelectedActiveOrder) // CONTROL
                {
                    Console.WriteLine($"CONTROL opened tables after deleting{item.Table}");
                }

                SaveToTempOrdersCSV(0);
                UpdateFile(ClosedOrdersContentsFilePath, temporaryOredersFilePath);
                SaveToTempOrdersCSV(1);
                UpdateFile(ActiveOrdersContentsFilePath, temporaryOredersFilePath);
                return tableNr;
            }
            else
            {
                PrintSomethingWrong();
                return tableNr = 0;
            }
        }

        public void SaveToTempOrdersCSV(int ActiveStatus)
        {
            using (StreamWriter sw = new StreamWriter(temporaryOredersFilePath))
            {
                if (ActiveStatus == 1)
                {
                    {
                        Console.WriteLine("\tall active contents foreach for renewing........");
                        foreach (var item in AllActiveOrdersContent)
                        {
                            string line = ConvertOrderContentToString(item);
                            sw.WriteLine(line);
                        }
                    }
                }
                else if (ActiveStatus == 0)
                {

                    {
                        Console.WriteLine("\tall closed contents foreach for renewing........");
                        foreach (var item in AllClosedOrdersContent)
                        {
                            string line = ConvertOrderContentToString(item);
                            sw.WriteLine(line);
                        }
                    }
                }
                else Console.WriteLine("SaveToTempOrdersCSV content oups");
            }
        }




        public string ChooseRightTargetFile(int ActiveStatus)
        {
            string rightFilePath;
            if (ActiveStatus == 1) //active
            {
                return rightFilePath = Path.Combine(currentDirectory, "ActiveOrdersContents.csv");
            }
            else if (ActiveStatus == 0) //closed
            {
                return rightFilePath = Path.Combine(currentDirectory, "ClosedOrdersContents.csv");
            }
            else
            {
                Console.WriteLine("neegzistuojantis order status");
                return rightFilePath = string.Empty;
            }
        }

        public void ImportAllFromCSV()
        {
            AllActiveOrdersContent.Clear();
            AllClosedOrdersContent.Clear();

            ImportAllActiveFromCSV();
            ImportAllClosedFromCSV();
        }

        public void ImportAllActiveFromCSV()
        {
            Console.WriteLine("\tCONTROL import open items");
            if (!IsFileAvailableToChange(ActiveOrdersContentsFilePath)) return;
            using (StreamReader sr = new StreamReader(ActiveOrdersContentsFilePath, Encoding.UTF8))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(';');
                    if (lineValues.Length == 6)
                    {
                        var recoveredContent = new OrderContent();

                        int table = int.Parse(lineValues[0].Trim());
                        string id = lineValues[1].Trim().ToString();
                        string itemName = lineValues[2].Trim().ToString();
                        double price = double.Parse(lineValues[3].Trim());
                        double itemQ = double.Parse(lineValues[4].Trim());
                        double linePrice = double.Parse(lineValues[5].Trim());
                        //CONTROL
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\tCONTROL nuskaitytos reiksmes -> {table}, {id}, {itemName}, {price}, {itemQ}, {linePrice}");
                        Console.ResetColor();

                        recoveredContent = ConvertLineToOrderContent(table, id, itemName, price, itemQ, linePrice);

                        AllActiveOrdersContent.Add(recoveredContent);
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

        public void ImportAllClosedFromCSV()
        {
            Console.WriteLine("\tCONTROL import closed items");
            if (!IsFileAvailableToChange(ClosedOrdersContentsFilePath)) return;

            using (StreamReader sr = new StreamReader(ClosedOrdersContentsFilePath, Encoding.UTF8))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(';');
                    if (lineValues.Length == 6)
                    {
                        var recoveredContent = new OrderContent();

                        int table = int.Parse(lineValues[0].Trim());
                        string id = lineValues[1].Trim().ToString();
                        string itemName = lineValues[2].Trim().ToString();
                        double price = double.Parse(lineValues[3].Trim());
                        double itemQ = int.Parse(lineValues[4].Trim());
                        double linePrice = double.Parse(lineValues[5].Trim());

                        //CONTROL
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\tCONTROL nuskaitytos reiksmes -> {table}, {id}, {itemName}, {price}, {itemQ}, {linePrice}");
                        Console.ResetColor();
                        //
                        recoveredContent = ConvertLineToOrderContent(table, id, itemName, price, itemQ, linePrice);

                        AllClosedOrdersContent.Add(recoveredContent);
                        currentLine++;
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

        public void ShowAllActiveContents()
        {
            Console.WriteLine("\tCONTROL all ACTIVE contents foreach........");
            if (AllActiveOrdersContent.Count > 0)
            {
                foreach (var item in AllActiveOrdersContent)
                {
                    Console.WriteLine($"table {item.Table} - {item.OrderID} \n{item.ItemName} - {item.ItemPrice} - {item.ItemQ} - {item.LinePrice}");
                }
            }
            else Console.WriteLine("\tNėra vykdommų užsakytų patiekalų");
        }

        public void ShowAllClosedContents()
        {
            Console.WriteLine("\tCONTROL all CLOSED contents foreach........");
            if (AllClosedOrdersContent.Count > 0)
            {

                foreach (var item in AllClosedOrdersContent)
                {
                    Console.WriteLine($"table {item.Table} - {item.OrderID} \n{item.ItemName} - {item.ItemPrice} - {item.ItemQ} - {item.LinePrice}");
                }
            }
            else Console.WriteLine("\tNėra pilnai užbaigtų užsakymų užsakytų patiekalų");
        }

        public void ClearAllLists()
        {
            AllActiveOrdersContent.Clear();
            AllClosedOrdersContent.Clear();
        }

        //savybes ir konstruktoriai
        public OrderContent() { }
        public OrderContent(int table, string orderID)
        {
            Table = table;
            OrderID = orderID;
            ItemName = "Staliuko rezervacija";
            ItemPrice = 0;
            ItemQ = 1;
            LinePrice = 0;
        }


        public OrderContent(int table, string orderID, string itemName, double itemPrice, double itemQ)
        {
            Table = table;
            OrderID = orderID;
            ItemName = itemName;
            ItemPrice = itemPrice;
            ItemQ = itemQ;
            LinePrice = itemPrice * itemQ;
        }
        public OrderContent(int table, string orderID, string itemName, double itemPrice, double itemQ, double linePrice) : this(table, orderID, itemName, itemPrice, itemQ)
        {
            LinePrice = linePrice;
        }

        public int Table { get; set; }
        public string OrderID { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public double ItemQ { get; set; }
        public double LinePrice { get; set; }


        public static List<OrderContent> AllActiveOrdersContent { get; set; } = new List<OrderContent>();
        public static List<OrderContent> AllClosedOrdersContent { get; set; } = new List<OrderContent>();

        public static string ActiveOrdersContentsFilePath = Path.Combine(currentDirectory, "ActiveOrdersContents.csv");
        public static string ClosedOrdersContentsFilePath = Path.Combine(currentDirectory, "ClosedOrdersContents.csv");
        public static string temporaryOredersFilePath = Path.Combine(currentDirectory, "tempOreders.csv");

    }
}
