using Pos.Domain.Repositories;
using Pos.Domain.Services;
using Pos.Domain.ValueObjects;
namespace Pos.Application.UseCases;
public class CalculateFinalGradeUseCase
{
    private readonly FinalGradeCalculator _calculator;
    private readonly IStudentRepository _students;
    public CalculateFinalGradeUseCase(FinalGradeCalculator calculator, IStudentRepository students)
    {
        _calculator = calculator;
        _students = students;
    }
    public (string Name, double Grade) Execute(StudentId studentId)
    {
        var student = _students.GetById(studentId)
            ?? throw new InvalidOperationException("Студент не найден");
        var grade = _calculator.CalculateFor(studentId);
        return (student.Name.Value, grade);
    }
}
