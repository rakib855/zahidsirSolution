namespace Project_API.ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = default!;
        public decimal AnnualCost { get; set; }
        public string Picture { get; set; } = default!;
        public bool Continuing { get; set; }
        public bool CanDelete { get; set; }
    }
}
