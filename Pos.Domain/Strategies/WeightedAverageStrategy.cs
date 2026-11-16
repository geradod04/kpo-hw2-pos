using Pos.Domain.Entities;
namespace Pos.Domain.Strategies;
public class WeightedAverageStrategy : IFinalGradeStrategy
{
    public string Name => "Взвешенное среднее";
    public double Calculate(IReadOnlyList<Evaluation> evaluations, IReadOnlyList<Criterion> criteria)
    {
        if (evaluations.Count == 0) return 0;
        var dict = criteria.ToDictionary(c => c.Id);
        double weightedSum = 0, weightSum = 0;
        foreach (var e in evaluations)
        {
            if (!dict.TryGetValue(e.CriterionId, out var c)) continue;
            var normalized = (double)e.Score.Value / c.MaxScore;
            weightedSum += normalized * c.Weight;
            weightSum += c.Weight;
        }
        return weightSum == 0 ? 0 : (weightedSum / weightSum) * 100;
    }
}
