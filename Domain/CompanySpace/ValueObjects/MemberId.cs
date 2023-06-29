using Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CompanySpace.ValueObjects;

public class MemberId : ValueObject
{
    public Guid Value { get; private set; }

    private MemberId(Guid id)
    {
        Value = id;
    }

    public static MemberId Create( Guid id) => new MemberId(id);

    public static MemberId CreateUnique() => new MemberId(Guid.NewGuid());
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private MemberId() { }
}
