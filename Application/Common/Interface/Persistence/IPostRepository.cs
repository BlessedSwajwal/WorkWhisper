using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interface.Persistence;

public interface IPostRepository
{
    Post Add(Post post);
    Comment AddComment(Guid postId, Comment comment);
    Post? GetById(PostId postId);
    Comment GetComment(PostId postId, CommentId commentId);
    List<Post> GetPostCollection(IReadOnlyCollection<PostId> postsIds);
    void UpvotePost(MemberId memberId, Post post);

    //  void UpvoteComment(MemberId memberId, CommentId commentId, PostId postId);

    /// <efcore>
    /// // assuming you have a list of Post ids called postIds
    /// List<int> postIds = new List<int> { 1, 2, 3 };
    ///
    /// fetch all the posts with the given Post ids in one go
    /// var posts = context.Posts
    ///                  .Where(p => postIds.Contains(p.Id))
    ///                  .ToList();
    /// 
    /// </efcore>
}
