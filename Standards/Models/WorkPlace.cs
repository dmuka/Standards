﻿namespace Standards.Models
{
    public class WorkPlace
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int RoomId { get; set; }
        public int SectorId { get; set; }
        public string ImagePath { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}