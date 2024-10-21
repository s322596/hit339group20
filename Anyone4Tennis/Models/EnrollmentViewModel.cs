namespace Anyone4Tennis.Models
{
    public class EnrollmentViewModel
    {
        public int MemberScheduleId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CoachName { get; set; }
    }
}

