using Application.Comments.Command.CreateComment;
using Contracts;
using Mapster;

namespace API.Mapping;

public class CreateCommentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CommentRequest, CreateCommentCommand>()
            .Map(dest => dest.PostId, src => src.PostId)
            .Map(dest => dest.Comment, src => src.CommentText);
    }
}
