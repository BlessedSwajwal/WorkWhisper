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
    public Post Add(Post post)
    {
        throw new NotImplementedException();
    }

    public Comment AddComment(Guid postId, Comment comment)
    {
        throw new NotImplementedException();
    }

    public Post? GetById(PostId postId)
    {
        throw new NotImplementedException();
    }

    public Comment GetComment(PostId postId, CommentId commentId)
    {
        throw new NotImplementedException();
    }

    public List<Post> GetPostCollection(IReadOnlyCollection<PostId> postsIds)
    {
        throw new NotImplementedException();
    }

    public void UpvotePost(MemberId memberId, Post post)
    {
        throw new NotImplementedException();
    }
}
