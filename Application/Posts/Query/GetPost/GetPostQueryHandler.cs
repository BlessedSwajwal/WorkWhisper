using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Query.GetPost;

public class GetPostQueryHandler : AbstractValidator<GetPostQuery>
{
	public GetPostQueryHandler()
	{
		RuleFor(x => x.PostId).NotEmpty();
	}
}
