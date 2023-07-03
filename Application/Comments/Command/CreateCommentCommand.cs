using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.Command;

public record CreateCommentCommand
(
    Guid PostId,
    string Comment
)  : IRequest;
