using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        


        // konstruktoriai, savybes
        public TableAndPlace() { }
        public TableAndPlace(int tableNr, Dictionary<int, int> nominalAndAvailiblePlaces)
        {
            TableNr = tableNr;
            //NominalAndAvailiblePlaces = new Dictionary<int, int> ();
            NominalAndAvailiblePlaces = nominalAndAvailiblePlaces;
            IsAvailible = true;
        }
        public int TableNr { get; set; }
        public Dictionary<int, int> NominalAndAvailiblePlaces { get; set; } = new Dictionary<int, int>();
        public bool IsAvailible { get; set; }
        public static Dictionary<int, int> CurrentAvailibleTablesHasPlaces { get; set; } = new Dictionary<int, int>();
        
        //public static Dictionary<int, Dictionary<int, int>> AllTabels = new Dictionary<int, Dictionary<int, int>>(); //maybe
        public static List<TableAndPlace> AllTabels = new List<TableAndPlace>();
        
        public static List<int> FreeNrForNewTables = new List<int> ();
    }
}
