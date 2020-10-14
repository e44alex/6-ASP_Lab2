using System;

namespace WebApplication1.Models
{
    public class Student
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string GroupNumber { get; set; }

        public int CourseNumber { get; set; }
    }
}