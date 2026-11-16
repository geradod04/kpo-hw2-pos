using Pos.Domain.Entities;
using Pos.Domain.Strategies;
namespace Pos.Infrastructure.Grading;
public class RoundingCalculatorDecorator : IFinalGradeStrategy
{
    private readonly IFinalGradeStrategy _inner;
    private readonly int _decimals;
    public RoundingCalculatorDecorator(IFinalGradeStrategy inner, int decimals = 2)
    {
        _inner = inner;
        _decimals = decimals;
    }
    public string Name => $"{_inner.Name} (округл. до {_decimals} зн.)";
    public double Calculate(IReadOnlyList<Evaluation> evaluations, IReadOnlyList<Criterion> criteria)
    {
        var raw = _inner.Calculate(evaluations, criteria);
        return Math.Round(raw, _decimals);
    }
}
