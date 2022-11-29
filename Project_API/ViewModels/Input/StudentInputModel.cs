using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_API.ViewModels.Input
{
    public class StudentInputModel
    {
        public int StudentId { get; set; }

        [Required, StringLength(50), Display(Name = "Student Name")]
        public string StudentName { get; set; } = default!;
        [Required, Column(TypeName = "money"), DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal AnnualCost { get; set; }
        [Display(Name = "Continuing?")]
        public bool Continuing { get; set; }
    }
}
