using System;

namespace ITI.Roomies.DAL.Spendings
{
    public class BudgetCatData
    {
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public string IconName { get; set; }
        public string CategoryName { get; set; }

        public DateTime Date1 { get; set; }

        public DateTime Date2 { get; set; }

        public int Amount { get; set; }

        public int CollocId { get; set; }
    }
}
