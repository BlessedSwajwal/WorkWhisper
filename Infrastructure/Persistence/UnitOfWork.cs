using Application.Common.Interface.Persistence;
using Infrastructure.Persistence.EFCoreRepositories;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly WorkWhisperDbContext _context;
    private readonly ISpaceRepository _spaceRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IPostRepository _postRepository;
    private bool _disposed = false;

    public ISpaceRepository SpaceRepository { get {
            if (_spaceRepository is null) 
                return new SpaceRepository(_context);
            return _spaceRepository; } }
    public IMemberRepository MemberRepository { get { 
            if(_memberRepository is null)
                return new MemberRepository(_context);
            return _memberRepository; } }
    public IPostRepository PostRepository { get { 
            if(_postRepository is null)
                return new PostRepository(_context);
            return _postRepository; } }
    public UnitOfWork(WorkWhisperDbContext ctx)
    {
        _context = ctx;
    }

    private void Dispose(bool disposing)
    {
        if(!this._disposed)
        {
            if(disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task SaveAsync()
    {
       await _context.SaveChangesAsync();
    }

}
