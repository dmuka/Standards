﻿using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;

namespace Standards.Core.Models.Departments
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int DepartmentId { get; set; }
        public IList<Room> Rooms { get; set; } = new List<Room>();
        public IList<WorkPlace> WorkPlaces { get; set; } = new List<WorkPlace>();
        public IList<Person> Persons { get; set; } = new List<Person>();
        public string Comments { get; set; } = null!;
    }
}