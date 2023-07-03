using Application.Posts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Space.Query.GetSpacePost;

public record GetSpacePostQuery(Guid SpaceId) : IRequest<List<PostResult>>;
