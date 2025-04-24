using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IStudentService
    {
        IEnumerable<StudentData> GetAllStudents();
        IEnumerable<StudentData> GetStudentsByName(string name);
        StudentData GetStudentById(int id);
        void AddStudent(StudentData student);
        void UpdateStudent(StudentData student);
        void DeleteStudent(int id);
    }
}
