using Application.Common.Interface.Persistence;
using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private static readonly List<Post> _posts = new List<Post>();
    public Post Add(Post post)
    {
        _posts.Add(post);
        return post;
    }

    public Comment AddComment(Guid postId, Comment comment)
    {
        var post = GetById(PostId.Create(postId));
        if (post is null) throw new Exception("Post not found.");
        post.AddComment(comment);
        return comment;
    }

    public Post? GetById(PostId postId)
    {
        return _posts.FirstOrDefault(post => post.Id == postId);
    }

    public List<Post> GetPostCollection(IReadOnlyCollection<PostId> postsIds)
    {
        var posts = new List<Post>();

        foreach (var postId in postsIds)
        {
            posts.Add(GetById(postId));
        }

        return posts;
    }

    public Comment GetComment(PostId postId, CommentId commentId) {
        var post = _posts.FirstOrDefault(p => p.Id == postId);
        if (post is null) throw new Exception("No such post found.");

        var comment = post.Comments.Where(c => c.Id == commentId).FirstOrDefault();
        if (comment is null) throw new Exception("No such comment.");
        return comment;
    }

}
