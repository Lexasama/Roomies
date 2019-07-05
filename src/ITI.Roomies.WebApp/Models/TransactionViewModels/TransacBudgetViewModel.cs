using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.TransactionViewModels
{
    public class TransacBudgetViewModel
    {
        public int TBudgetId { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [DataType( DataType.DateTime )]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int RoomieId { get; set; }
    }
}
