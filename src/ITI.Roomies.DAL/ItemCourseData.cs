namespace ITI.Roomies.DAL
{
    public class ItemCourseData
    {
        public int ItemId { get; set; }

        public int ItemPrice { get; set; }

        public string ItemName { get; set; }

        public bool ItemSaved { get; set; }

        public int ItemQuantite { get; set; }

        public bool ItemBought { get; set; }

        public int CollocId { get; set; }

        public int CourseId { get; set; }

        public int RoomieId { get; set; }

        public string FirstName { get; set; }
    }
}
