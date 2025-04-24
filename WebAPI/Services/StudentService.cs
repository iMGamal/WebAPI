using WebAPI.Repositories;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IEnumerable<StudentData> GetAllStudents()
        {
            return _studentRepository.GetAllStudents();
        }

        public IEnumerable<StudentData> GetStudentsByName(string name)
        {
            return _studentRepository.GetStudentsByName(name);
        }

        public StudentData GetStudentById(int id)
        {
            return _studentRepository.GetStudentById(id);
        }

        public void AddStudent(StudentData student)
        {
            _studentRepository.AddStudent(student);
        }

        public void UpdateStudent(StudentData student)
        {
            _studentRepository.UpdateStudent(student);
        }

        public void DeleteStudent(int id)
        {
            _studentRepository.DeleteStudent(id);
        }
    }
}