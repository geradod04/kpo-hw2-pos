using Pos.Domain.ValueObjects;
namespace Pos.Domain.Entities;
public class Student
{
    public StudentId Id { get; }
    public FullName Name { get; }
    public Student(StudentId id, FullName name)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
