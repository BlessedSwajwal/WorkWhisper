using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;

public record CommentResult(Guid CommentId, string Comment, Guid CommenterId, int upvotes);
