using System.ComponentModel.DataAnnotations.Schema;

namespace Anyone4Tennis.Models
{
    public class MemberSchedule
    {
        public int MemberScheduleId { get; set; }
        [ForeignKey("Member")]
        public string? MemberFK { get; set; }
        public virtual Member? Member { get; set; }

        [ForeignKey("Schedules")]
        public int ScheduleID { get; set; }
        public virtual Schedules Schedules { get; set; }
    }
}