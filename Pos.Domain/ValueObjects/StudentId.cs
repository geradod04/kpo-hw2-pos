namespace Pos.Domain.ValueObjects;
public record StudentId
{
    public Guid Value { get; }
    public StudentId(Guid value) { Value = value; }
    public static StudentId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
