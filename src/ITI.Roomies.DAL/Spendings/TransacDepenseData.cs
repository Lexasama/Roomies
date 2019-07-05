using System;

namespace ITI.Roomies.DAL
{
    public class TransacDepenseData
    {
        public int TDepenseId { get; set; }

        public int Price { get; set; }

        public DateTime Date { get; set; }

        public int SRoomieId { get; set; }

        public int RRoomieId { get; set; }
    }
}
