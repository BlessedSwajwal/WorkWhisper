using Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CompanySpace.ValueObjects;

public class CommentId : ValueObject
{
    public Guid Value { get; private set; }

    private CommentId(Guid id) {
        Value = id;
    }

    public static CommentId Create(Guid id) => new CommentId(id);
    public static CommentId CreateUnique() => new CommentId(Guid.NewGuid());
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private CommentId() { }
}
