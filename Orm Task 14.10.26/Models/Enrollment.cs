using System;
using System.Collections.Generic;
using System.Text;

namespace Orm_Task_14._10._26.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public decimal? Grade { get; set; }

        public Student Student { get; set; } = null!;

        public Course Course { get; set; } = null!;


    }
}
