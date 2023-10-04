using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Domain.Post.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.EventHandlers;

public class HandlePostCreated : INotificationHandler<PostCreated>
{
    private readonly IUnitOfWork _unitOfWork;

    public HandlePostCreated(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Saves the PostId of the post created in the respective CompanySpace object.
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(PostCreated notification, CancellationToken cancellationToken)
    {
        //Get the space on which post was created.
        var member = _unitOfWork.MemberRepository.GetMemberById(notification.post.OwnerId);
        if (member is null) throw new SpaceNotFoundException();

        //Add the postId to the space.
        member.AddPost(notification.post.Id.Value);

        return;
    }
}
