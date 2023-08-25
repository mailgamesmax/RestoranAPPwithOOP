using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6.Models
{
    internal class OrderSummary : CommonFunctions, ISimilarFuntions
    {
        public void CreateOrder()
        {
            //var newOrder = new OrderSummary(table, status, totalPrice, notification, created, terminated, id);
            var newOrder = new OrderSummary();
            int inputTable = InputPredataForNewOrder();
            string newID = NrRandomGeneratorOnTime();
            newOrder = new OrderSummary(inputTable, newID);

            var newOrederContent = new OrderContent();
            newOrederContent.CreateActiveOrderContent(inputTable, newID);

            AddNewToCSV(newOrder, 1);
            AllActiveOrders.Add(newOrder);
        }

        public void AddItemToOrder()
        {
            Console.WriteLine("\tCONTROL add item to order\n");

            //            var dish = new Dish();
            var drink = new Drink();
            double addItemAndPrice;

            int inputTableNr = InputTableNr();
            var selectedOrder = AllActiveOrders.FirstOrDefault(table => table.Table == inputTableNr);
            string selectedOrderID = selectedOrder.UniqID;

            string kindOfItem = InputDishOrDrink();
            if (kindOfItem == "patiekalas")
            {
                AddDishItem(selectedOrder, inputTableNr, selectedOrderID);
            }
            else if (kindOfItem == "gerimas")
            {
                AddDishItem(selectedOrder, inputTableNr, selectedOrderID);
            }
            else
            {
                BackToWelcome();
                return;
            }
            SaveToTempOrdersCSV();
            UpdateFile(ActiveOrdersSummaryFilePath, temporaryOredersFilePath);

            //if (kindOfItem == "gerimas") AddDrinkItem(inputTableNr, selectedOrderID);
        }

        public void AddDishItem(OrderSummary selectedOrder, int inputTableNr, string selectedOrderID)
        {
            var dish = new Dish();
            var content = new OrderContent();
            string? repeatAction;

            do
            {
                dish.SearchByName();
                double addItemAndPrice = content.AddIDishItemToOrder(inputTableNr, selectedOrderID);
                selectedOrder.TotalPrice += addItemAndPrice;
                Console.Write("\npridėti kitą? (+) ");
                repeatAction = Console.ReadLine();
            }
            while (repeatAction == "+");
        }

        public void AddDrinkItem(OrderSummary selectedOrder, int inputTableNr, string selectedOrderID)
        {
            var drink = new Drink();
            var content = new OrderContent();
            string? repeatAction;

            do
            {
                drink.SearchByName();
                double addItemAndPrice = content.AddIDrinkItemToOrder(inputTableNr, selectedOrderID);
                selectedOrder.TotalPrice += addItemAndPrice;
                Console.Write("\npridėti kitą? (+) ");
                repeatAction = Console.ReadLine();
            }
            while (repeatAction == "+");
        }

        public int InputTableNr()
        {
            //string inputChoice;            
            Console.Write("Užsakymo stalo nr?");
            int inputTableNr = ConvertInputToIntIfPositive();
            if (inputTableNr == 0) BackToWelcome();
            else return inputTableNr;

            PrintSomethingWrong();
            BackToWelcome();
            return 0;
        }

        public string InputDishOrDrink()
        {
            string inputChoice;
            do
            {
                Console.WriteLine("Pridėti patiekalą - 1, pridėti gėrimą - 2, nutraukti įvedimą - 0");
                inputChoice = Console.ReadLine();
                //if (inputChoice == "0") return string.Empty;
            }
            while (inputChoice != "0" && inputChoice != "1" && inputChoice != "2");
            if (inputChoice == "1") return "patiekalas";
            if (inputChoice == "2") return "gerimas";

            BackToWelcome();
            return string.Empty;
        }

        /*        public OrderSummary SelectActiveOrderByTable()
                {
                    var selectedOrder = new OrderSummary();

                    var content = new OrderContent();
                    int actualTableNr = content.CloseActiveOrderContent();

                    var table = new TableAndPlace();
                    table.DeOcupideTableEverywere(actualTableNr);

                    var selectedOrder = AllActiveOrders.FirstOrDefault(table => table.Table == actualTableNr);
                    return selectedOrder;
                }*/

        public void CloseActiveOrder()
        {
            var selectedOrder = new OrderSummary();
            selectedOrder = CloseContentDeocupideTable();

            AllClosedOrders.Add(selectedOrder);
            AddNewToCSV(selectedOrder, 0);

            AllActiveOrders.Remove(selectedOrder);
            SaveToTempOrdersCSV();
            UpdateFile(ActiveOrdersSummaryFilePath, temporaryOredersFilePath);
        }

        public OrderSummary CloseContentDeocupideTable()
        {
            var content = new OrderContent();
            int actualTableNr = content.CloseActiveOrderContent();

            var table = new TableAndPlace();
            table.DeOcupideTableEverywere(actualTableNr);

            var selectedOrder = AllActiveOrders.FirstOrDefault(table => table.Table == actualTableNr);
            return selectedOrder;
        }

        public void SaveToTempOrdersCSV()
        {
            using (StreamWriter sw = new StreamWriter(temporaryOredersFilePath))
            {
                Console.WriteLine("\tCONTROL all open orders foreach for renewing........");
                foreach (var item in AllActiveOrders)
                {
                    string line = ConvertOrderSummaryToString(item);
                    sw.WriteLine(line);
                }
            }
        }


        public void AddNewToCSV(OrderSummary newOrderSummary, int ActiveStatus)
        {
            string targetFilePath = ChooseRightTargetFile(ActiveStatus);
            using (StreamWriter sw = new StreamWriter(targetFilePath, true, Encoding.UTF8))
            {
                string line = ConvertOrderSummaryToString(newOrderSummary);
                sw.WriteLine(line);
            }
        }
        public int InputPredataForNewOrder()
        {
            Console.Write("Svečių (aktualių vietų) skaičius? ");
            string inputTablePlaces = Console.ReadLine();
            var table = new TableAndPlace();
            TableAndPlace.MyFilteredTabels.Clear();
            table.SelectAvailableTablesByNominalPlacesFromFile(inputTablePlaces);
            table.ShowTablesInfo(TableAndPlace.MyFilteredTabels);
            Console.WriteLine("Pasirinkite stalą rezervacijai: ");
            int inputTableNr = int.Parse(Console.ReadLine());
            // pabaigti stalo  valdymą stalo klaseje........
            table.OcupideTableEverywere(inputTableNr, inputTablePlaces);

            return inputTableNr;
        }


        public void ImportAllFromCSV()
        {
            AllActiveOrders.Clear();
            AllClosedOrders.Clear();

            ImportAllActiveFromCSV();
            ImportAllClosedFromCSV();
        }

        public void ImportAllActiveFromCSV()
        {
            Console.WriteLine("\tCONTROL import open orders");
            if (!IsFileAvailableToChange(ActiveOrdersSummaryFilePath)) return;
            using (StreamReader sr = new StreamReader(ActiveOrdersSummaryFilePath, Encoding.UTF8))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(';');
                    if (lineValues.Length == 7)
                    {
                        var recoveredOrder = new OrderSummary();

                        int table = int.Parse(lineValues[0].Trim());
                        if (Enum.TryParse(lineValues[1].Trim(), out ProcessingStage status)) ;
                        else
                        {
                            Console.WriteLine($"nepavyksta atkurti order status {currentLine} elutėje ");
                            return;
                        }
                        //string status = lineValues[1].Trim();
                        double totalPrice = double.Parse(lineValues[2].Trim());
                        string notification = lineValues[3].Trim().ToString();
                        DateTime created = DateTime.Parse(lineValues[4].Trim());
                        DateTime terminated = DateTime.Parse(lineValues[5].Trim());
                        string id = lineValues[6].Trim().ToString();

                        //CONTROL
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\tCONTROL nuskaitytos reiksmes -> {table} - {status} - {totalPrice} -  {notification} - {created} - {terminated} ");
                        Console.ResetColor();

                        recoveredOrder = ConvertLineToOrderSummary(table, status, totalPrice, notification, created, terminated, id);

                        AllActiveOrders.Add(recoveredOrder);
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

        public void ImportAllClosedFromCSV()
        {
            Console.WriteLine("\tCONTROL import closed orders");
            if (!IsFileAvailableToChange(ClosedOrdersSummaryFilePath)) return;
            using (StreamReader sr = new StreamReader(ClosedOrdersSummaryFilePath, Encoding.UTF8))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(';');
                    if (lineValues.Length == 7)
                    {
                        var recoveredOrder = new OrderSummary();

                        int table = int.Parse(lineValues[0].Trim());
                        ProcessingStage status = ProcessingStage.Closed;
                        double totalPrice = double.Parse(lineValues[2].Trim());
                        string notification = lineValues[3].Trim().ToString();
                        DateTime created = DateTime.Parse(lineValues[4].Trim());
                        DateTime terminated = DateTime.Parse(lineValues[5].Trim());
                        string id = lineValues[6].Trim().ToString();

                        //CONTROL
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\tCONTROL nuskaitytos reiksmes -> {table} - {status} - {totalPrice} -  {notification} - {created} - {terminated} - {id} ");
                        Console.ResetColor();

                        recoveredOrder = ConvertLineToOrderSummary(table, status, totalPrice, notification, created, terminated, id);

                        AllClosedOrders.Add(recoveredOrder);
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

        public OrderSummary ConvertLineToOrderSummary(int table, ProcessingStage status, double totalPrice, string notification, DateTime created, DateTime terminated, string id)
        {
            var orderSummary = new OrderSummary(table, status, totalPrice, notification, created, terminated, id);
            return orderSummary;
        }

        public string ConvertOrderSummaryToString(OrderSummary orderSummary)
        {
            {
                string orderSummaryToString = $"{orderSummary.Table}; {orderSummary.Status}; {orderSummary.TotalPrice}; {orderSummary.Notifications}; {orderSummary.CreationTime}; {orderSummary.TerminationTime}; {orderSummary.UniqID}";
                return orderSummaryToString;
            }
        }

        public string ChooseRightTargetFile(int ActiveStatus)
        {
            string rightFilePath;
            if (ActiveStatus == 1) //active
            {
                return rightFilePath = Path.Combine(currentDirectory, "ActiveOrdersSummary.csv");
            }
            else if (ActiveStatus == 0) //closed
            {
                return rightFilePath = Path.Combine(currentDirectory, "ClosedOrdersSummary.csv");
            }
            else
            {
                Console.WriteLine("neegzistuojantis order status");
                return rightFilePath = string.Empty;
            }
        }

        public string NrRandomGeneratorOnTime() //datos konv i stringa
        {
            DateTime creatingTime = DateTime.Now;

            string generatedNr = string.Join("", creatingTime.Month, creatingTime.Day, creatingTime.Hour, creatingTime.Minute, creatingTime.Second);
            return generatedNr;
        }

        public void ClearAllLists()
        {
            AllActiveOrders.Clear();
            AllClosedOrders.Clear();
        }

        //savybes ir konstruktoriai
        public OrderSummary() { }

        public OrderSummary(int table, string uniqID)
        {
            Table = table;
            Status = ProcessingStage.NOT_STARTED;
            TotalPrice = 0;
            Notifications = "-";
            CreationTime = DateTime.Now;
            TerminationTime = CreationTime;
            UniqID = uniqID;
        }

        public OrderSummary(int table, string uniqID, string notification) : this(table, uniqID)
        {
            Notifications = notification;
        }

        public OrderSummary(int table, ProcessingStage status, double totalPrice, string notification, DateTime created, DateTime terminated, string uniqID)
        {
            Table = table;
            Status = status;
            TotalPrice = totalPrice;
            Notifications = notification;
            CreationTime = created;
            TerminationTime = terminated;
            UniqID = uniqID;
        }

        public enum ProcessingStage
        {
            NOT_STARTED,
            Producing,
            OnHold,
            Closed
        }

        public int Table { get; set; }
        public ProcessingStage Status { get; set; }
        public double TotalPrice { get; set; }
        public string Notifications { get; set; }
        public DateTime CreationTime { get; set; } = new DateTime();
        public DateTime TerminationTime { get; set; } = new DateTime();
        public string UniqID { get; set; }
        //public List<OrderContent> FullContent { get; set; } = new List<OrderContent>();

        //public DateTime CreationTime { get; set; } = new DateTime();
        //public DateTime TerminationTime { get; set; } = new DateTime();
        public static List<OrderSummary> AllActiveOrders { get; set; } = new List<OrderSummary>();
        public static List<OrderSummary> AllClosedOrders { get; set; } = new List<OrderSummary>();
        //
        public static string ActiveOrdersSummaryFilePath = Path.Combine(currentDirectory, "ActiveOrdersSummary.csv");
        public static string ClosedOrdersSummaryFilePath = Path.Combine(currentDirectory, "ClosedOrdersSummary.csv");
        public static string temporaryOredersFilePath = Path.Combine(currentDirectory, "tempOreders.csv");

    }
}
