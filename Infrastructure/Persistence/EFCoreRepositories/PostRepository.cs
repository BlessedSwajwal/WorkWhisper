using Application.Common.Interface.Persistence;
using Domain.Member.ValueObjects;
using Domain.Post;
using Domain.Post.Entity;
using Domain.Post.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EFCoreRepositories;

public class PostRepository : IPostRepository
{
    private readonly WorkWhisperDbContext _context;

    public PostRepository(WorkWhisperDbContext context)
    {
        _context = context;
    }

    public Post Add(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
        return post;
    }

    public Comment AddComment(Guid postId, Comment comment)
    {
        _context.Posts.FirstOrDefault(p => p.Id == PostId.Create(postId))!.AddComment(comment);
        
        _context.SaveChanges();
        return comment;
    }

    public Post? GetById(PostId postId)
    {
        return _context.Posts.FirstOrDefault(p=>p.Id==postId);
    }

    public Comment GetComment(PostId postId, CommentId commentId)
    {
        return _context.Posts.FirstOrDefault(p => p.Id == postId).Comments
            .FirstOrDefault(c => c.Id ==  commentId);
    }

    public List<Post> GetPostCollection(IReadOnlyCollection<PostId> postsIds)
    {
        var posts = _context.Posts.Where(p => postsIds.Contains(p.Id));
        return posts.ToList();
    }

    public void UpvotePost(MemberId memberId, Post post)
    {
        post.Upvote(memberId);
        _context.Posts.Attach(post);
        _context.Posts.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
    }
}
