using Pos.Domain.Entities;
namespace Pos.Domain.Strategies;
public interface IFinalGradeStrategy
{
    string Name { get; }
    double Calculate(IReadOnlyList<Evaluation> evaluations, IReadOnlyList<Criterion> criteria);
}
