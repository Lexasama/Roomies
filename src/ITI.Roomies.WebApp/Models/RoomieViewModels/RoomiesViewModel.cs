using System;
using System.ComponentModel.DataAnnotations;


namespace ITI.Roomies.WebApp.Models.RoomieModel
{
	public class RoomiesViewModel
	{

        //public int RoomieId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType( DataType.Date )]
        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public string RoomiePic { get; set; }

        //public string Email { get; set; }

        //public string Description { get; set; }
    }
}
