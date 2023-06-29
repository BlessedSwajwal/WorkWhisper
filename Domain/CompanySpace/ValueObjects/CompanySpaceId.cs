using Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CompanySpace.ValueObjects;

public sealed class CompanySpaceId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private CompanySpaceId(Guid id) {
        Value = id;
    }

    public static CompanySpaceId Create(Guid id)
    {
        return new(id);
    }

    public static CompanySpaceId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private CompanySpaceId() { }
}
