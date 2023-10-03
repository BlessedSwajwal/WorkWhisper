using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Query.GetPost;

public class GetPostQueryValidator : AbstractValidator<GetPostQuery>
{
	public GetPostQueryValidator()
	{
		RuleFor(x => x.PostId).NotEmpty();
	}
}
