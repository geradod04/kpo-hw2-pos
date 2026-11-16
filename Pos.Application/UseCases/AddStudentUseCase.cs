using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;
namespace Pos.Application.UseCases;
public class AddStudentUseCase
{
    private readonly IStudentRepository _repo;
    public AddStudentUseCase(IStudentRepository repo) { _repo = repo; }
    public StudentId Execute(string fullName)
    {
        var student = new Student(StudentId.New(), new FullName(fullName));
        _repo.Save(student);
        return student.Id;
    }
}
