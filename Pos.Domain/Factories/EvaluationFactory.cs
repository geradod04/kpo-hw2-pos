using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;
namespace Pos.Domain.Factories;
public class EvaluationFactory
{
    private readonly ICriterionRepository _criteria;
    private readonly IStudentRepository _students;
    public EvaluationFactory(ICriterionRepository criteria, IStudentRepository students)
    {
        _criteria = criteria;
        _students = students;
    }
    public Evaluation Create(StudentId studentId, CriterionId criterionId, int scoreValue, string? comment = null)
    {
        var student = _students.GetById(studentId)
            ?? throw new InvalidOperationException($"Студент {studentId} не найден в репозитории");
        var criterion = _criteria.GetById(criterionId)
            ?? throw new InvalidOperationException($"Критерий {criterionId} не найден в рубрикаторе");
        var score = new Score(scoreValue, criterion.MaxScore);
        return new Evaluation(
            EvaluationId.New(),
            studentId,
            criterionId,
            score,
            DateTime.Now,
            comment);
    }
}
