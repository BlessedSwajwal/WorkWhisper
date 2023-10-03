namespace Application.Common.Interface.Persistence;

public interface IUnitOfWork : IDisposable
{
    public ISpaceRepository SpaceRepository { get;}
    public IPostRepository PostRepository { get;}
    public IMemberRepository MemberRepository { get;}
    Task SaveAsync();
}
