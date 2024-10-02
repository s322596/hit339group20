using Microsoft.AspNetCore.Identity;

namespace Anyone4Tennis.Models
{
    public class Coach : ApplicationUser
    {
        public int CoachId { get; set; }
        public string Biography { get; set; }
        public string Photo { get; set; }
    }
}
