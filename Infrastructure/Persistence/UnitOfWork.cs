using Application.Common.Interface.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly WorkWhisperDbContext _context;
    private ISpaceRepository _spaceRepository;
    private IMemberRepository _memberRepository;
    private IPostRepository _postRepository;
    private bool _disposed = false;

    public ISpaceRepository SpaceRepository { get { return _spaceRepository; } }
    public IMemberRepository MemberRepository { get { return _memberRepository; } }
    public IPostRepository PostRepository { get { return _postRepository; } }
    public UnitOfWork(WorkWhisperDbContext ctx, ISpaceRepository spaceRepository, IMemberRepository memberRepository, IPostRepository postRepository)
    {
        _context = ctx;
        _spaceRepository = spaceRepository;
        _memberRepository = memberRepository;
        _postRepository = postRepository;
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
