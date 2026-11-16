using Pos.Domain.Entities;
namespace Pos.Domain.Strategies;
public class SumStrategy : IFinalGradeStrategy
{
    public string Name => "Сумма баллов";
    public double Calculate(IReadOnlyList<Evaluation> evaluations, IReadOnlyList<Criterion> criteria)
    {
        return evaluations.Sum(e => e.Score.Value);
    }
}
