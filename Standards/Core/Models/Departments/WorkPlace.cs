﻿using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class WorkPlace
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int RoomId { get; set; }
        public Person Responcible { get; set; } = new Person();
        public string ImagePath { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}