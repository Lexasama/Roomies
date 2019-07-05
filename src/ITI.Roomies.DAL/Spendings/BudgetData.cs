using System;

namespace ITI.Roomies.DAL
{
    public class BudgetData
    {
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }

        public bool Custom { get; set; }

        public DateTime Date1 { get; set; }

        public DateTime Date2 { get; set; }

        public int Amount { get; set; }

    }
}
