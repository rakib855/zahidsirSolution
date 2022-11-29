using System.ComponentModel.DataAnnotations;

namespace Project_API.ViewModels
{
    public class SubjectViewModel
    {
        public int SubjectId { get; set; }
        [Required, StringLength(50), Display(Name = "Subject Name")]
        public string SubjectName { get; set; } = default!;
        [Required, StringLength(150)]
        public string Title { get; set; } = default!;
        [Required]
        public int TotalHour { get; set; } = default!;
        public bool CanDelete { get; set; }
    }
}
