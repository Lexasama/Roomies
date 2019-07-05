using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.SpendingsViewModels
{
    public class BudgetViewModel
    {
       
        public int BudgetId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date1 { get; set; }

        [Required]
        [DataType( DataType.Date )]
        public DateTime Date2 { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int CollocId { get; set; }
    }
}
