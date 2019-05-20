using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.ItemModel
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }

        [Required]
        public int ItemPrice { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int CourseId { get; set; }


        public int RoomieId { get; set; }
    }
}
