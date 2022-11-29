using Project_API.Models;

namespace Project_API.ViewModels
{
    public class TeacherViewModel
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = default!;
        
        public DateTime DateOfBirth { get; set; } = default!;
        
        public TeacherType TeacherType { get; set; }
       
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = default!;
    }
}
