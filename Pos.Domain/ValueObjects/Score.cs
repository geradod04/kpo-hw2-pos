namespace Pos.Domain.ValueObjects;
public record Score
{
    public int Value { get; }
    public int MaxScore { get; }
    public Score(int value, int maxScore)
    {
        if (maxScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxScore), "Максимум должен быть > 0");
        if (value < 0 || value > maxScore)
            throw new ArgumentException($"Балл {value} вне диапазона [0, {maxScore}]");
        Value = value;
        MaxScore = maxScore;
    }
}
