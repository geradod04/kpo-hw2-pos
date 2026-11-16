using Pos.Domain.Entities;
using Pos.Domain.ValueObjects;
namespace Pos.Domain.Repositories;
public interface IStudentRepository
{
    void Save(Student student);
    Student? GetById(StudentId id);
    IReadOnlyList<Student> GetAll();
}
