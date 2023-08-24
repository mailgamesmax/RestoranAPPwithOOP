using RestoranOOPonNet6.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6
{
    internal class OrderContent : CommonFunctions, ISimilarFuntions
    {
        public void CreateActiveOrderContent(int table, int orderID, string itemName, double itemPrice, int itemQ)
        {
            var newOrderContent = new OrderContent(table, orderID, itemName, itemPrice, itemQ);
            AddNewToCSV(newOrderContent);
            AllActiveOrdersContent.Add(newOrderContent);           
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
                string orderContentToString = ($"{newOrderContent.Table}; {newOrderContent.OrderID}; {newOrderContent.ItemName}; {newOrderContent.ItemPrice}; {newOrderContent.ItemQ}; {newOrderContent.LinePrice}");
                return orderContentToString;
            }
        }

        public OrderContent ConvertLineToOrderContent(int table, int orderID, string itemName, double itemPrice, int itemQ, double linePrice)
        {
            var orderContent = new OrderContent(table, orderID, itemName, itemPrice, itemQ, linePrice);
            return orderContent;
        }

        

        public List<OrderContent> SelectActiveByTable()
        {
            Console.Write("Užsakymo stalo nr? (0 - norint nutraukt veiksmą) ");
            int tableNr = int.Parse(Console.ReadLine());
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
        
        public void CloseActiveOrderContent() 
        {

            List<OrderContent> allItemsOfSelectedActiveOrder = new List<OrderContent>();
            allItemsOfSelectedActiveOrder = SelectActiveByTable();
            
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
            }
            else
            {
                PrintSomethingWrong();
                return;
            }
        }

        public void SaveToTempOrdersCSV(int ActiveStatus)
        {
            //string targetFilePath = ChooseRightTargetFile(ActiveStatus);
            using (StreamWriter sw = new StreamWriter(temporaryOredersFilePath))
            {
                if (ActiveStatus == 1)
                {
                    {
                        Console.WriteLine("\tall active contents foreach for renewing........");
                        foreach (var item in OrderContent.AllActiveOrdersContent)
                        {
                            string line = ConvertOrderContentToString(item);
                            sw.WriteLine(line);
                        }
                    }
                }
                else if (ActiveStatus == 0)
                {
                        //using (StreamWriter sw = new StreamWriter(temporaryAssortimentFilePath))
                        {
                            Console.WriteLine("\tall closed contents foreach for renewing........");
                            foreach (var item in OrderContent.AllClosedOrdersContent)
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
                        int id = int.Parse(lineValues[1].Trim());
                        string itemName = lineValues[2].Trim().ToString();
                        double price = double.Parse(lineValues[3].Trim());
                        int itemQ = int.Parse(lineValues[4].Trim());
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
                        int id = int.Parse(lineValues[1].Trim());
                        string itemName = lineValues[2].Trim().ToString();
                        double price = double.Parse(lineValues[3].Trim());
                        int itemQ = int.Parse(lineValues[4].Trim());
                        double linePrice = double.Parse(lineValues[5].Trim());
                        
                        //CONTROL
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\tCONTROL nuskaitytos reiksmes -> {table}, {id}, {itemName}, {price}, {itemQ}, {linePrice}");
                        Console.ResetColor();
                        //
                        recoveredContent = ConvertLineToOrderContent(table, id, itemName, price, itemQ, linePrice);

                        AllClosedOrdersContent.Add(recoveredContent);
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
            if (AllClosedOrdersContent.Count>0)
            {

                foreach (var item in AllClosedOrdersContent)
                {
                    Console.WriteLine($"table {item.Table} - {item.OrderID} \n{item.ItemName} - {item.ItemPrice} - {item.ItemQ} - {item.LinePrice}");
                }
            }
            else Console.WriteLine("\tNėra pilnai užbaigtų užsakymų užsakytų patiekalų");
        }

        //savybes ir konstruktoriai
        public OrderContent() { }
        public OrderContent(string name) { }


        public OrderContent(int table, int orderID, string itemName, double itemPrice, int itemQ)
        {
            Table = table;
            OrderID = orderID;
            ItemName = itemName; 
            ItemPrice = itemPrice;        
            ItemQ = itemQ;
            LinePrice = itemPrice * itemQ;
        }
        public OrderContent(int table, int orderID, string itemName, double itemPrice, int itemQ, double linePrice) : this(table, orderID, itemName, itemPrice, itemQ)
        {
            LinePrice = linePrice;
        }

        public int Table { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public int ItemQ { get; set; }
        public double LinePrice { get; set; }


        public static List<OrderContent> AllActiveOrdersContent { get; set; } = new List<OrderContent>();
        public static List<OrderContent> AllClosedOrdersContent { get; set; } = new List<OrderContent>();
        
        public static string ActiveOrdersContentsFilePath = Path.Combine(currentDirectory, "ActiveOrdersContents.csv");
        public static string ClosedOrdersContentsFilePath = Path.Combine(currentDirectory, "ClosedOrdersContents.csv");
        public static string temporaryOredersFilePath = Path.Combine(currentDirectory, "tempOreders.csv");

    }
}
