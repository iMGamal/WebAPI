using WebAPI.DataAccess;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly WebAPIContext _context;

        public StudentRepository(WebAPIContext context)
        {
            _context = context;
        }

        public IEnumerable<StudentData> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public IEnumerable<StudentData> GetStudentsByName(string name)
        {
            return _context.Students.Where(s => s.StudentName.Contains(name)).ToList();
        }

        public StudentData GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public void AddStudent(StudentData student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(StudentData student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }
    }
}
