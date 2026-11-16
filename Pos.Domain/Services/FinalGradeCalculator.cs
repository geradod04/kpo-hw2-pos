using Pos.Domain.Repositories;
using Pos.Domain.Strategies;
using Pos.Domain.ValueObjects;
namespace Pos.Domain.Services;
public class FinalGradeCalculator
{
    private readonly IEvaluationRepository _evaluations;
    private readonly ICriterionRepository _criteria;
    private readonly IFinalGradeStrategy _strategy;
    public FinalGradeCalculator(
        IEvaluationRepository evaluations,
        ICriterionRepository criteria,
        IFinalGradeStrategy strategy)
    {
        _evaluations = evaluations;
        _criteria = criteria;
        _strategy = strategy;
    }
    public double CalculateFor(StudentId studentId)
    {
        var evals = _evaluations.GetByStudent(studentId);
        var criteria = _criteria.GetAll();
        return _strategy.Calculate(evals, criteria);
    }
}
