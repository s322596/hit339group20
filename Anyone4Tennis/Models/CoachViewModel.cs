﻿namespace Anyone4Tennis.Models.ViewModels
{
    public class CoachViewModel
    {
        public string CoachId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public byte[]? Photo { get; set; }
    }
}
