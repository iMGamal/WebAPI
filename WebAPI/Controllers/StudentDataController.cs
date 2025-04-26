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
        public async Task<IActionResult> Add([FromForm] StudentData student)
        {
            student.StudentId = 0;

            if (student.StudentImage != null && student.StudentImage.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(student.StudentImage.FileName);

                var imagesFolder = Path.Combine("wwwroot", "images");
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), imagesFolder, uniqueFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await student.StudentImage.CopyToAsync(stream);
                }

                student.ImagePath = "/images/" + uniqueFileName;
            }

            _service.AddStudent(student);
            return CreatedAtAction(nameof(GetById), new { studentId = student.StudentId }, student);
        }

        [HttpPut("{studentId}")]
        public async Task<IActionResult> Edit(int studentId, [FromForm] StudentUpdateModel updateModel)
        {
            var existingStudent = _service.GetStudentById(studentId);
            if (existingStudent == null)
                return NotFound($"Student with ID {studentId} not found");

            // Update only non-null properties
            if (updateModel.StudentName != null)
                existingStudent.StudentName = updateModel.StudentName;

            if (updateModel.StudentAge.HasValue)
                existingStudent.StudentAge = updateModel.StudentAge.Value;

            if (updateModel.StudentAddress != null)
                existingStudent.StudentAddress = updateModel.StudentAddress;

            if (updateModel.StudentPhoneNumber != null)
                existingStudent.StudentPhoneNumber = updateModel.StudentPhoneNumber;

            if (updateModel.StudentBirthDate.HasValue)
                existingStudent.StudentBirthDate = updateModel.StudentBirthDate.Value;

            // Handle file upload
            if (updateModel.StudentImage != null && updateModel.StudentImage.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingStudent.ImagePath))
                {
                    var oldImagePath = Path.Combine("wwwroot", existingStudent.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                // Fixed: Changed student.StudentImage to updateModel.StudentImage
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(updateModel.StudentImage.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    // Fixed: Changed student.StudentImage to updateModel.StudentImage
                    await updateModel.StudentImage.CopyToAsync(stream);
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
