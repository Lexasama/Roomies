using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.SpendingsViewModels
{
    public class BudgetCatViewModel
    {
        [Required]
        public int CollocId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
