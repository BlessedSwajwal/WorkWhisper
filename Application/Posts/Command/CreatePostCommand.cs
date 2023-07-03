using Application.Posts.Common;
using Domain.CompanySpace.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Command;

public record CreatePostCommand
(string title, string body, bool isPrivate) : IRequest<PostResult>;
