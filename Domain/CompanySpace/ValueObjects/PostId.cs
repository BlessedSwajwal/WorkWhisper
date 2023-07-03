using Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CompanySpace.ValueObjects;

public sealed class PostId : ValueObject
{
    public Guid Value { get; private set; }

    private PostId(Guid value)
    {
        Value = value;
    }

    public static PostId Create(Guid id) => new(id);
    public static PostId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    private PostId() { }
}
