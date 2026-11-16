namespace Pos.Domain.ValueObjects;
public record CriterionId
{
    public Guid Value { get; }
    public CriterionId(Guid value) { Value = value; }
    public static CriterionId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
