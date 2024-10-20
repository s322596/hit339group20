using Anyone4Tennis.Models;
namespace Anyone4Tennis.Models
{
    public class Schedules
    {
        public int SchedulesID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsFullDay { get; set; }
        public string CoachId { get; set; }

        // Navigation Property for Coach
        public virtual Coach Coach { get; set; }

        public ICollection<MemberSchedule> MemberSchedules { get; set; } = new List<MemberSchedule>();
    }
}
