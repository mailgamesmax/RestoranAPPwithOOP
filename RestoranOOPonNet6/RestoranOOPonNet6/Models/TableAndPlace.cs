using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal class TableAndPlace
    {
        public TableAndPlace() { }
        public TableAndPlace(int tableNr, int tableHasPLace)
        {
            TableNr = tableNr;
            TableHasPLace = tableHasPLace;
            IsAvailible = true;
        }
        public int TableNr { get; set; }
        public int TableHasPLace { get; set; }
        public bool IsAvailible { get; set; }
        public static Dictionary<int, int> TablesPlaces { get; set; } = new Dictionary<int, int>();
        public static List<TableAndPlace> FreeTables = new List<TableAndPlace>();
        public static List<TableAndPlace> BusyTables = new List<TableAndPlace>();
    }
}
