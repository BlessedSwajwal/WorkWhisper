using Domain.CompanySpace.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Common;

public record PostResult(Guid id, string Title, string Body, Guid SpaceId, bool isPrivate, IReadOnlyCollection<Comment> comments);
