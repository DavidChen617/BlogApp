using Domain.Common;

namespace Domain.Posts;

public sealed class PostId : ValueObject
{
    public Guid Value { get; }

    private PostId(Guid value) => Value = value;

    public static PostId New() => new(Guid.NewGuid());
    public static PostId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
