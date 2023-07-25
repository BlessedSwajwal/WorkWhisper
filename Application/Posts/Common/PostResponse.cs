using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Common;

public record PostResponse(
    Guid PostId,
    string Title,
    string Body,
    Guid SpaceId,
    bool IsPrivate,
    int upvotes,
    List<CommentResult> CommentResult);
