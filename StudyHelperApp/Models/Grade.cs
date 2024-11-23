using System;

namespace StudyHelperApp.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SubjectName { get; set; }
    }
}
