using Application.Posts.Common;
using MediatR;

namespace Application.Posts.Command;

public record CreatePostCommand(
    string Title, 
    string Body, 
    bool IsPrivate
) : IRequest<PostResult>;
