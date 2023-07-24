using Application.Posts.Command.CreatePost;
using Contracts;
using Mapster;

namespace API.Mapping;

public class CreatePostMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePostRequest, CreatePostCommand>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.IsPrivate, src => src.IsPrivate);
    }
}
