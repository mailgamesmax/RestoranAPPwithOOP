using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RestoranOOPonNet6.Models
{
    internal class TableAndPlace : CommonFunctions //, ISimilarFuntions
    {
        public TableAndPlace CreateNewTable() 
        {
            int inputTableNr = 0;
            int inputAvailiblePlaces = 0;
            string? repeatAction;
            var createdTable = new TableAndPlace();
            do
            {
                CheckForLastTablesNr();
                CheckMissedTableNr();
                InputNewTableData(ref inputTableNr, ref inputAvailiblePlaces);
                Console.WriteLine("creating.........\n");

                createdTable = new TableAndPlace(inputTableNr, new Dictionary<int, int> { { inputAvailiblePlaces, inputAvailiblePlaces } });

                UpdateAllTabels(createdTable);
                UpdateCurrentAvailibleTablesHasPlaces(inputTableNr, inputAvailiblePlaces);
                Console.WriteLine($"sukurto stalo nr: {createdTable.TableNr}, nominualus/prieinamas vietų sk: {createdTable.NominalAndAvailiblePlaces.Keys.First()} / {createdTable.NominalAndAvailiblePlaces.Values.First()}");

                RecToCSV(createdTable);

                Console.WriteLine();
                Console.Write("priėti dar vieną stalą? (+) ");
                repeatAction = Console.ReadLine();
            }
            while (repeatAction == "+");            
            return createdTable; 
        }

        public void CheckForLastTablesNr()
        {
            int currentLargestTableNr = 0;
            if (AllTabels.Count > 0)
            {
                currentLargestTableNr = currentLargestTableNr = AllTabels.Max(t => t.TableNr);
                Console.Write("esamas didžiausas turimų stalų nr -> " + currentLargestTableNr + ". ");
            }
            else { Console.WriteLine("Nėra apskaitomų stalų."); }
        }

        public void CheckMissedTableNr()
        { 
            if (FreeNrForNewTables.Count > 0)
            {           
                Console.WriteLine("Kiti neužimti nr: ");
                for (int i = 0; i < FreeNrForNewTables.Count; i++)
                {
                    Console.Write(FreeNrForNewTables[i]);
                    if (i > FreeNrForNewTables.Count)
                        Console.Write(", ");
                    else { Console.WriteLine("\n"); }
                }
            }
        }

        public void InputNewTableData(ref int inputTableNr, ref int inputAvailiblePlaces)
        {
            bool ConvertedToIntTableNr = false;
            bool ConvertedToIntPlacesQ = false;

            do
            {
                Console.Write("Priskirkite naujam stalui nr: ");
                ConvertedToIntTableNr = int.TryParse(Console.ReadLine(), out inputTableNr);
                int inputedNr = inputTableNr;
                Console.Write("Kiek vietų turės naujas stalas? ");
                ConvertedToIntPlacesQ = int.TryParse(Console.ReadLine(), out inputAvailiblePlaces);
                if (AllTabels.Any(anyTable => anyTable.TableNr == inputedNr))
                {
                    ConvertedToIntTableNr = false;
                }

                if (!ConvertedToIntTableNr && !ConvertedToIntTableNr)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\tpatikrinkite įvestus pasirinkimus ir bandykite dar kartą\n");
                    Console.ResetColor();
                }
            }
                while (ConvertedToIntTableNr == false && ConvertedToIntTableNr == false) ;
        }

        public void UpdateAllTabels(TableAndPlace tableAndPlace) 
        {
            AllTabels.Add(tableAndPlace);
            Console.WriteLine("Bendras stalų sąrašas atnaujintas.");
        }

        public void UpdateCurrentAvailibleTablesHasPlaces(int tableNr, int availibleQ)
        {
            if (CurrentAvailibleTablesHasPlaces.TryAdd(tableNr, availibleQ))
            Console.WriteLine($"Laisų vietų skaičius prie stalo {tableNr} - atnaujintas.");
            else if (CurrentAvailibleTablesHasPlaces.ContainsKey(tableNr)) 
            {
                CurrentAvailibleTablesHasPlaces[tableNr] = availibleQ;
                Console.WriteLine($"Laisų vietų skaičius stalo {tableNr} - pakeistas.");
            }
            else { Console.WriteLine($"Nenumatyta klaida - nepavyko atnaujinti {tableNr} stalo laisvų vietų skaičiaus."); }
        }
        
        public void ShowAllTablesInfo()
        {

            Console.WriteLine("foreach method... - visi stalai ");
            foreach (var table in AllTabels)
            {
                string tableAvailibilty;
                if (table.IsAvailible) { tableAvailibilty = "neužimtas"; }             
                else { tableAvailibilty = "užimtas"; }
                Console.WriteLine($"-> stalo nr {table.TableNr}, {tableAvailibilty}, laisvų vietų - {table.NominalAndAvailiblePlaces.Values.First()}");
            }

        }

        public static void RecToCSV(TableAndPlace table)
        {
            //string tableFilePath = currentDirectory + "Tables.csv";
            string tableFilePath = Path.Combine(currentDirectory, "Tables.csv");
            using (StreamWriter sw = new StreamWriter(tableFilePath, true))
            {
                    string line = ConvertObjectsToString(table);
                    sw.WriteLine(line); 
                
            }
        }



        public static string ConvertObjectsToString(TableAndPlace table)         
        {   
            string tablesToString = ($"{table.TableNr}, {table.NominalAndAvailiblePlaces.Keys.First()}, {table.NominalAndAvailiblePlaces.Values.First()}, {table.IsAvailible}");
            return tablesToString;
        }

        public void ImportAllFromCSV()
        {
            string tableFilePath = Path.Combine(currentDirectory, "Tables.csv");
            using (StreamReader sr = new StreamReader(tableFilePath))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(',');
                    if (lineValues.Length == 4) 
                    {
                        var createdTable = new TableAndPlace();

                        int tableNr = int.Parse(lineValues[0].Trim());
                        int nominalPlaces = int.Parse(lineValues[1].Trim());
                        int availablePlaces = int.Parse(lineValues[2].Trim());
                        bool isAvailable = bool.Parse(lineValues[3].Trim());
                        ConvertLineToTableFromFile(tableNr, nominalPlaces, availablePlaces, isAvailable, out createdTable);
                        UpdateAllTabels(createdTable);
                        UpdateCurrentAvailibleTablesHasPlaces(nominalPlaces, availablePlaces);
                    }
                    else { Console.WriteLine($"{currentLine} eilutėje klaidingas stalo savybių kiekis"); }
                }
            }
        }



        public static void ConvertLineToTableFromFile(int inputTableNr, int nominalPlaces, int availiblePlaces, bool isAvailible, out TableAndPlace createdTable)
        {

         //   var table = new TableAndPlace();
           // var createdTable = new TableAndPlace();
                createdTable = new TableAndPlace(inputTableNr, new Dictionary<int, int> { { nominalPlaces, availiblePlaces }}, isAvailible);

/*            table.UpdateAllTabels(createdTable);
            table.UpdateCurrentAvailibleTablesHasPlaces(inputTableNr, availiblePlaces);
*/
            //return createdTable;
        }

        public void SelectActualTablesFromCSV()
        {
            Console.WriteLine("Įveskite (jei reikia) aktualią info apie stalą.");
            string inputTableNr, inputNominalPlaces, inputActualPlaces, inputAvailibleStatus;
            InputTableFilterCriteria(out inputTableNr, out inputNominalPlaces, out inputActualPlaces, out inputAvailibleStatus);
            if (!string.IsNullOrEmpty(inputTableNr)) return;
            if (!string.IsNullOrEmpty(inputAvailibleStatus)) SelectTableByAvailabilityStatusFromFile(inputAvailibleStatus);
            if (!string.IsNullOrEmpty(inputNominalPlaces)) 
            {
                if (MyFilteredTabels.Count> 0)
                {
                    MyFilteredTabels = MyFilteredTabels.Where(t => t.NominalAndAvailiblePlaces.Keys.First() >= int.Parse(inputNominalPlaces)).ToList(); 
                }
                else SelectByNominalPlacesFromFile(inputNominalPlaces);
            }
            if (!string.IsNullOrEmpty(inputAvailibleStatus))
            {
                if (MyFilteredTabels.Count > 0)
                {
                    MyFilteredTabels = MyFilteredTabels.Where(t => t.NominalAndAvailiblePlaces.Keys.First() >= int.Parse(inputNominalPlaces)).ToList();
                }
                else SelectByAvailablePlacesFromFile(inputActualPlaces);
            }

            Console.WriteLine("Rūšiavimo rezultatas --->\n");

           //int tableNr = string.IsNullOrEmpty(inputTableNr) ? int.Parse(lineValues[0].Trim()) : int.Parse(inputTableNr); // pvz isiminimui 

            ShowTableInfo(MyFilteredTabels);
        }

        public void SelectTableByAvailabilityStatusFromFile(string inputAvailibleStatus)
        {
            string tableFilePath = Path.Combine(currentDirectory, "Tables.csv");
            using (StreamReader sr = new StreamReader(tableFilePath))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(',');
                    if (lineValues.Length == 4)
                    {
                        var createdTable = new TableAndPlace();
                        if (bool.Parse(lineValues[3].Trim()) == bool.Parse(inputAvailibleStatus))
                        {
                            int tableNr = int.Parse(lineValues[0].Trim());
                            int nominalPlaces = int.Parse(lineValues[1].Trim());
                            int availablePlaces = int.Parse(lineValues[2]);
                            bool isAvailable = bool.Parse(lineValues[3]);
                            ConvertLineToTableFromFile(tableNr, nominalPlaces, availablePlaces, isAvailable, out createdTable);
                            MyFilteredTabels.Add(createdTable);
                        }
                    }
                    else { Console.WriteLine($"{currentLine} eilutėje klaidingas stalo savybių kiekis"); }
                }
            }
        }

        public void SelectByAvailablePlacesFromFile(string inputActualPlaces)
        {
            string tableFilePath = Path.Combine(currentDirectory, "Tables.csv");
            using (StreamReader sr = new StreamReader(tableFilePath))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(',');
                    if (lineValues.Length == 4)
                    {
                        var createdTable = new TableAndPlace();
                        if (int.Parse(lineValues[1].Trim()) >= int.Parse(inputActualPlaces))
                        {
                            int tableNr = int.Parse(lineValues[0].Trim());
                            int nominalPlaces = int.Parse(lineValues[1].Trim());
                            int availablePlaces = int.Parse(lineValues[2]);
                            bool isAvailable = bool.Parse(lineValues[3]);
                            ConvertLineToTableFromFile(tableNr, nominalPlaces, availablePlaces, isAvailable, out createdTable);
                            MyFilteredTabels.Add(createdTable);
                        }
                    }
                    else { Console.WriteLine($"{currentLine} eilutėje klaidingas stalo savybių kiekis"); }
                }
            }
        }

        public void SelectByNominalPlacesFromFile(string inputNominalPlaces)
        {
            string tableFilePath = Path.Combine(currentDirectory, "Tables.csv");
            using (StreamReader sr = new StreamReader(tableFilePath))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(',');
                    if (lineValues.Length == 4)
                    {
                        var createdTable = new TableAndPlace();
                        if (int.Parse(lineValues[1].Trim()) >= int.Parse(inputNominalPlaces))
                        {
                            int tableNr = int.Parse(lineValues[0].Trim());
                            int nominalPlaces = int.Parse(lineValues[1].Trim());
                            int availablePlaces = int.Parse(lineValues[2]);
                            bool isAvailable = bool.Parse(lineValues[3]);
                            ConvertLineToTableFromFile(tableNr, nominalPlaces, availablePlaces, isAvailable, out createdTable);
                            MyFilteredTabels.Add(createdTable);
                        }
                    }
                    else { Console.WriteLine($"{currentLine} eilutėje klaidingas stalo savybių kiekis"); }
                }
            }
        }

        public void SelectByTableNrFromFile(string inputTableNr)
        {
            string tableFilePath = Path.Combine(currentDirectory, "Tables.csv");
            using (StreamReader sr = new StreamReader(tableFilePath))
            {
                string line;
                int currentLine = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lineValues = line.Split(',');
                    if (lineValues.Length == 4)
                    {
                        var createdTable = new TableAndPlace();
                        if (int.Parse(lineValues[0].Trim()) == int.Parse(inputTableNr))
                        {
                        int tableNr = int.Parse(lineValues[0].Trim());
                        int nominalPlaces = int.Parse(lineValues[1].Trim());
                        int availablePlaces = int.Parse(lineValues[2]);
                        bool isAvailable = bool.Parse(lineValues[3]);
                        ConvertLineToTableFromFile(tableNr, nominalPlaces, availablePlaces, isAvailable, out createdTable);
                        MyFilteredTabels.Add(createdTable);
                        }                        
                    }
                    else { Console.WriteLine($"{currentLine} eilutėje klaidingas stalo savybių kiekis"); }
                }
            }

            ShowTableInfo(MyFilteredTabels);
        }


        public void InputTableFilterCriteria(out string inputTableNr, out string inputNominalPlaces, out string inputActualPlaces, out string inputAvailibleStatus)
        {
            Console.WriteLine("*PASTABA: jeigu kriterijus neaktualus, tiesiog spauskite ENTER.\n");
            Console.Write("Stalo nr? ");
            inputTableNr = Console.ReadLine();
            if (!string.IsNullOrEmpty(inputTableNr))
            {
                inputNominalPlaces = inputActualPlaces = inputAvailibleStatus = "";
                SelectByTableNrFromFile(inputTableNr);
                return;

            }
            Console.Write("Nominalus vietų sk? ");
            inputNominalPlaces = Console.ReadLine();
            Console.Write("Faktinis vietų sk.? ");
            inputActualPlaces = Console.ReadLine();
            Console.Write("Užimtumo statusas? (1 - laisvas, 2 - užimtas) ");
            string inputTableStatus = Console.ReadLine();
            if (inputTableStatus == "1") inputAvailibleStatus = "True";
            else if (inputTableStatus == "2") inputAvailibleStatus = "False";
            else inputAvailibleStatus = string.Empty;
            Console.WriteLine();

            Console.WriteLine($"Aktualūs atrankos kriterijai: \n Stalo nr -> {inputTableNr}, nominalus vietų skaičius -> {inputNominalPlaces}, faktinis vietų skaičius -> {inputActualPlaces}, užimtumo statusas -> {inputAvailibleStatus}");
            Console.WriteLine();
        }

        public void CollectSelectedTables(TableAndPlace tableAndPlace)
        {
            MyFilteredTabels.Add(tableAndPlace);
        }

        public void ShowTableInfo(List<TableAndPlace> anyTablesList)
        {

            Console.WriteLine("foreach method........");

            if (anyTablesList.Count>0) 
            {            
                foreach (var table in anyTablesList)
                {
                    string tableAvailibilty;
                    if (table.IsAvailible) { tableAvailibilty = "neužimtas"; }
                    else { tableAvailibilty = "užimtas"; }
                    Console.WriteLine($"-> stalo nr {table.TableNr}, {tableAvailibilty}, viso vietų/liko laisvų vietų - {table.NominalAndAvailiblePlaces.Keys.First()}/ {table.NominalAndAvailiblePlaces.Values.First()}");
                }
            }
            else Console.WriteLine("sorry budy, nothing");
        }

        public static void FilterTablesByFreePlaces(int neededPlace)
        {
            int inputChoice = 0;
            while (inputChoice != 1 && inputChoice != 2) 
            {
            Console.WriteLine("Ieškoti tik tarp laisvų stalų (1) / tikrinti visus stalus (2)");
            bool correctInput = int.TryParse(Console.ReadLine(), out inputChoice);
                if (inputChoice != 1 && inputChoice != 2) Console.WriteLine("e, dv nesimaivom\n");
                else Console.WriteLine();
            }
            if (inputChoice == 1)
            {
                var filteredTables = AllTabels.Where(t => (t.IsAvailible == true && t.NominalAndAvailiblePlaces.Keys.First() >= neededPlace));
                foreach (var oneTable in filteredTables)
                {
                    Console.WriteLine($"Table Nr: {oneTable.TableNr}, sėdimų vietų: {oneTable.NominalAndAvailiblePlaces.Values.First()}");
                }
            }
            else
            {
                var filteredTables = AllTabels.Where(t => t.NominalAndAvailiblePlaces.Values.First() >= neededPlace).OrderBy(t =>t.IsAvailible).ThenBy(t => t.NominalAndAvailiblePlaces.Values.First());
                foreach (var oneTable in filteredTables)
                { string status;
                    if (oneTable.IsAvailible == true) status = "NEužimtas";
                    else status = "Užimtas";
                    Console.WriteLine($"Table Nr: {oneTable.TableNr}, laisvų vietų: {oneTable.NominalAndAvailiblePlaces.Values.First()}, statusas - {status}");
                }
            }
            Console.WriteLine();
        }


        // konstruktoriai, savybes
        public TableAndPlace() { }
        public TableAndPlace(int tableNr, Dictionary<int, int> nominalAndAvailiblePlaces)
        {
            TableNr = tableNr;
            //NominalAndAvailiblePlaces = new Dictionary<int, int> ();
            NominalAndAvailiblePlaces = nominalAndAvailiblePlaces;
            IsAvailible = true;
        }
        public TableAndPlace(int tableNr, Dictionary<int, int> nominalAndAvailiblePlaces, bool isAvailible) : this(tableNr, nominalAndAvailiblePlaces)
        {
            IsAvailible = isAvailible;
        }
        public int TableNr { get; set; }
        public Dictionary<int, int> NominalAndAvailiblePlaces { get; set; } = new Dictionary<int, int>();
        public bool IsAvailible { get; set; }
        public static Dictionary<int, int> CurrentAvailibleTablesHasPlaces { get; set; } = new Dictionary<int, int>();
        
        public static List<TableAndPlace> MyFilteredTabels = new List<TableAndPlace>();
        //public static Dictionary<int, Dictionary<int, int>> AllTabels = new Dictionary<int, Dictionary<int, int>>(); //maybe
        public static List<TableAndPlace> AllTabels = new List<TableAndPlace>();
        
        public static List<int> FreeNrForNewTables = new List<int> ();
    }
}
