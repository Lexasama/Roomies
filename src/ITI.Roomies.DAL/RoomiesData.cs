using System;

namespace ITI.Roomies
{
    public class RoomiesData
    {
        public int RoomieId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string RoomiePic { get; set; }

        public string Description { get; set; }

        public byte[] Password { get; set; }

        public string GoogleRefreshToken { get; set; }

        public string GoogleId { get; set; }
    }
}
