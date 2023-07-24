using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Command.UpvotePost;

public record UpvotePostCommand
(
    Guid memberId,
    Guid postId
) : IRequest;
