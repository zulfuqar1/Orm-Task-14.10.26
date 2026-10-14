using System;
using System.Collections.Generic;
using System.Text;

namespace Orm_Task_14._10._26.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Teacher> Teachers { get; set; } = new();
        public List<Course> Courses { get; set; } = new();
    }
}
