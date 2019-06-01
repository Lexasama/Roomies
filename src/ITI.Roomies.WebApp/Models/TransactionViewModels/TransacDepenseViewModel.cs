using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.TransactionViewModels
{
    public class TransacDepenseViewModel
    {

        public int TDepenseId { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [DataType( DataType.DateTime )]
        public DateTime Date { get; set; }

        [Required]
        public int SRoomieId { get; set; }

        [Required]
        public int RRoomieId { get; set; }
    }
}
