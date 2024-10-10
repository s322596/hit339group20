namespace Anyone4Tennis.Models
{
    public class Schedules
    {
        public int SchedulesID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsFullDay { get; set; }
    }
}
