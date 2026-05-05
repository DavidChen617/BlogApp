using Domain.Common;

namespace Domain.Comments;

public sealed class CommentId : ValueObject
{
    public Guid Value { get; }

    private CommentId(Guid value) => Value = value;

    public static CommentId New() => new(Guid.NewGuid());
    public static CommentId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
