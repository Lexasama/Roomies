using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.ItemModel
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }

        public int ItemPrice { get; set; }

        [Required]
        public string ItemName { get; set; }

        public int CourseId { get; set; }

        public int RoomieId { get; set; }

        public int CollocId { get; set; }

        public bool ItemSaved { get; set; }

        //[Required]
        //public bool IsRepeated { get; set; } 
    }
}
