using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI.Models
{
    public class StudentData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int StudentAge { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? StudentImage { get; set; }
        public string StudentAddress { get; set; }
        public string StudentPhoneNumber { get; set; }
        public DateOnly StudentBirthDate { get; set; }
    }
}
