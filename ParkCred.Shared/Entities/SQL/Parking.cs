using SQLite;

namespace ParkCred.Shared.Entities.SQL
{
    public class Parking
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Conditions { get; set; }

        public string WorkTime { get; set; }

        public string WeekendConditions { get; set; }
    }
}
