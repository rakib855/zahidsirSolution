using Project_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_API.ViewModels.Input
{
    public class TeacherInputModel
    {
        [Required]
        public int TeacherId { get; set; }

        [Required, StringLength(50), Display(Name = "Teacher Name")]
        public string TeacherName { get; set; } = default!;
        [Required, Column(TypeName = "date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; } = default!;
        [Required, EnumDataType(typeof(TeacherType))]
        public TeacherType TeacherType { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public List<TeacherStudent> TeacherStudents { get; }
    }
}
