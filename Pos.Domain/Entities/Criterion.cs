using Pos.Domain.ValueObjects;
namespace Pos.Domain.Entities;
public class Criterion
{
    public CriterionId Id { get; }
    public string Name { get; }
    public int MaxScore { get; }
    public double Weight { get; }
    public Criterion(CriterionId id, string name, int maxScore, double weight)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название критерия обязательно");
        if (maxScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxScore), "Максимум должен быть > 0");
        if (weight < 0 || weight > 1)
            throw new ArgumentOutOfRangeException(nameof(weight), "Вес в диапазоне [0, 1]");
        Id = id;
        Name = name;
        MaxScore = maxScore;
        Weight = weight;
    }
}
