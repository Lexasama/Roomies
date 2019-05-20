using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Roomies.DAL
{
    public class TransactionData
    {
        public int TransactId { get; set; }

        public string TransacDesc { get; set; }

        public int TransacPrice { get; set; }

        public DateTime TransacDate { get; set; }

        public int CollocId { get; set; }

        public int RoomieId { get; set; }
    }
}
