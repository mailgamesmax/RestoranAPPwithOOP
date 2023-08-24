using RestoranOOPonNet6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RestoranOOPonNet6.Models.Assortiment;

namespace RestoranOOPonNet6
{
    internal class OrderSummary : CommonFunctions, ISimilarFuntions
    {


        public void ImportAllFromCSV()
        {
            throw new NotImplementedException();
        }
        //savybes ir konstruktoriai
        public OrderSummary() { }
        public OrderSummary(string name) { }


        public OrderSummary(int table, int uniqID)
        {
            Table = table;
            Status = ProcessingStage.NOT_STARTED;
            TotalPrice = 0;
            Notifications = "-";
            CreationTime = DateTime.Now;
            TerminationTime = CreationTime;
            UniqID = uniqID;
        }

        public OrderSummary(int table, int uniqID, string notification) : this(table, uniqID)
        {
            Notifications = notification;
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
        public int UniqID { get; set; }
        //public List<OrderContent> FullContent { get; set; } = new List<OrderContent>();

        //public DateTime CreationTime { get; set; } = new DateTime();
        //public DateTime TerminationTime { get; set; } = new DateTime();
        public static List<OrderSummary> AllOrders { get; set; } = new List<OrderSummary>();
        //
        public static string ClosedOrdersSummaryFilePath = Path.Combine(currentDirectory, "ClosedOrdersSummary.csv");
        public static string temporaryOredersFilePath = Path.Combine(currentDirectory, "tempOreders.csv");

    }
}
