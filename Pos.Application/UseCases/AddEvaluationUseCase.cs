using Pos.Domain.Factories;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;
namespace Pos.Application.UseCases;
public class AddEvaluationUseCase
{
    private readonly EvaluationFactory _factory;
    private readonly IEvaluationRepository _repo;
    public AddEvaluationUseCase(EvaluationFactory factory, IEvaluationRepository repo)
    {
        _factory = factory;
        _repo = repo;
    }
    public void Execute(StudentId studentId, CriterionId criterionId, int score, string? comment)
    {
        var evaluation = _factory.Create(studentId, criterionId, score, comment);
        _repo.Save(evaluation);
    }
}
