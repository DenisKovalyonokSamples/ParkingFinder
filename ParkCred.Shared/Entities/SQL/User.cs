using SQLite;
using System;

namespace ParkCred.Shared.Entities.SQL
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActiveSession { get; set; }

        public int ActiveParkingId { get; set; }

        public int Balance { get; set; }

        public int SessionStatus { get; set; }

        public int AutoModeStatus { get; set; }

        public DateTime? SessionStartTime { get; set; }
    }
}
