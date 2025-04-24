using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDataController : ControllerBase
    {
        private readonly ILogger<StudentDataController> _logger;
        private readonly IStudentService _service;

        public StudentDataController(ILogger<StudentDataController> logger, IStudentService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _service.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("byname/{name}")]
        public IActionResult GetByName(string name)
        {
            var students = _service.GetStudentsByName(name);
            if (students == null || !students.Any())
                return NotFound($"No students found with name: {name}");

            return Ok(students);
        }

        [HttpGet("{studentId}")]
        public IActionResult GetById(int studentId)
        {
            var student = _service.GetStudentById(studentId);
            if (student == null)
                return NotFound($"Student with ID {studentId} not found");

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] StudentData student, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                var directory = Path.GetDirectoryName(imagePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                student.ImagePath = "/images/" + uniqueFileName;
            }

            _service.AddStudent(student);
            return CreatedAtAction(nameof(GetById), new { studentId = student.StudentId }, student);
        }

        [HttpPut("{studentId}")]
        public async Task<IActionResult> Edit([FromForm] StudentData student, int studentId, IFormFile imageFile = null)
        {
            if (studentId != student.StudentId)
                return BadRequest("ID in URL doesn't match ID in student data");

            var existingStudent = _service.GetStudentById(studentId);
            if (existingStudent == null)
                return NotFound($"Student with ID {studentId} not found");

            existingStudent.StudentName = student.StudentName;
            existingStudent.StudentAge = student.StudentAge;
            existingStudent.StudentAddress = student.StudentAddress;
            existingStudent.StudentPhoneNumber = student.StudentPhoneNumber;
            existingStudent.StudentBirthDate = student.StudentBirthDate;

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingStudent.ImagePath))
                {
                    var oldImagePath = Path.Combine("wwwroot", existingStudent.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existingStudent.ImagePath = "/images/" + uniqueFileName;
            }

            _service.UpdateStudent(existingStudent);
            return Ok(existingStudent);
        }

        [HttpDelete("{studentId}")]
        public IActionResult Delete(int studentId)
        {
            var student = _service.GetStudentById(studentId);
            if (student == null)
                return NotFound($"Student with ID {studentId} not found");

            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                var imagePath = Path.Combine("wwwroot", student.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _service.DeleteStudent(studentId);
            return NoContent();
        }
    }
}
