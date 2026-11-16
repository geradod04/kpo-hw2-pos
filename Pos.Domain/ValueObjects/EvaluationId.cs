namespace Pos.Domain.ValueObjects;
public record EvaluationId
{
    public Guid Value { get; }
    public EvaluationId(Guid value) { Value = value; }
    public static EvaluationId New() => new(Guid.NewGuid());
}
