using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.Command.UpvoteComment;

public record UpvoteCommentCommand
(
    Guid UserId,
    Guid PostId,
    Guid CommentId
) : IRequest;
