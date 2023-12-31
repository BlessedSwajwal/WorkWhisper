﻿using Application.Posts.Common;
using MediatR;

namespace Application.Posts.Command.CreatePost;

public record CreatePostCommand(
    string Title,
    string Body,
    bool IsPrivate
) : IRequest<PostResponse>;
