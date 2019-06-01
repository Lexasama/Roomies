using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.TransactionViewModels
{
    public class TransactionViewModel
    {
        [Required]
        public int Price { get; set; }
        [Required]
        public int CollocId { get; set; }

        [Required]
        [DataType( DataType.DateTime )]
        public DateTime Date { get; set; }

        public int CategoryId { get; set; }
        public int RoomieId { get; set; }
    }
}
