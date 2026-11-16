namespace Pos.Domain.ValueObjects;
public record FullName
{
    public string Value { get; }
    public FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ФИО не может быть пустым");
        Value = value;
    }
    public override string ToString() => Value;
}
