namespace WebAPI.Models
{
    public class StudentUpdateModel
    {
        public string? StudentName { get; set; }
        public int? StudentAge { get; set; }
        public IFormFile? StudentImage { get; set; }
        public string? StudentAddress { get; set; }
        public string? StudentPhoneNumber { get; set; }
        public DateOnly? StudentBirthDate { get; set; }
    }
}
