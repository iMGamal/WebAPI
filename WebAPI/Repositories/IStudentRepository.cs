using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface IStudentRepository
    {
        IEnumerable<StudentData> GetAllStudents();
        IEnumerable<StudentData> GetStudentsByName(string name);
        StudentData GetStudentById(int id);
        void AddStudent(StudentData student);
        void UpdateStudent(StudentData student);
        void DeleteStudent(int id);
    }
}
