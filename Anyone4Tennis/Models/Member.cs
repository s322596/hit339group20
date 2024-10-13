using Microsoft.AspNetCore.Identity;

namespace Anyone4Tennis.Models
{
    public class Member : ApplicationUser
    {
        public int MemberId { get; set; }
        public bool Active { get; set; }
    }
}
