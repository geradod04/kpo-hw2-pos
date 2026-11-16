using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;
namespace Pos.Infrastructure.Persistence;
public class InMemoryStudentRepository : IStudentRepository
{
    private readonly Dictionary<StudentId, Student> _data = new();
    public void Save(Student s) => _data[s.Id] = s;
    public Student? GetById(StudentId id) => _data.GetValueOrDefault(id);
    public IReadOnlyList<Student> GetAll() => _data.Values.ToList();
}
