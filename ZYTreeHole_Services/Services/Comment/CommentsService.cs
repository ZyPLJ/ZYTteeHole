using CodeLab.Share.ViewModels.Response;
using ZYTreeHole_Models;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;

namespace ZYTreeHole_Services.Services;

public class CommentsService: ICommentsService
{
    private readonly MyDbContext  _myDbContext;

    public CommentsService(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }
    public async Task<ZyComments> CreateCommentAsync(CommentDto commentDto)
    {
        var comment = new ZyComments
        {
            ParentId = commentDto.ParentId,
            UserId = commentDto.UserId,
            Content = commentDto.Content,
            IsNeedAudit = commentDto.IsNeedAudit,
            Visible = commentDto.Visible,
            CreateTime = DateTime.Now,
        };
        await _myDbContext.comments.AddAsync(comment);
        await _myDbContext.SaveChangesAsync();
        return comment;
    }

    public async Task<ZyComments> Accept(ZyComments comment, string? reason = null)
    {
        var data = _myDbContext.comments.Find(comment.Id);
        data.Visible = true;
        data.IsNeedAudit = true;
        data.Reason = reason;
        await _myDbContext.SaveChangesAsync();
        return comment;
    }
    public async Task<ZyComments> Reject(ZyComments comment, string reason) {
        var data = _myDbContext.comments.Find(comment.Id);
        comment.Visible = false;
        comment.IsNeedAudit = false;
        comment.Reason = reason;
        await _myDbContext.SaveChangesAsync();
        return comment;
    }
}