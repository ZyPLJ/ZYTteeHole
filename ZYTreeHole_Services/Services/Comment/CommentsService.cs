using CodeLab.Share.ViewModels;
using CodeLab.Share.ViewModels.Response;
using Microsoft.EntityFrameworkCore;
using ZYTreeHole_Models;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole_Models.ViewModels.Responses;
using ZYTreeHole_Share;

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

    public async Task<(List<ZyComments>,PaginationMetadata)> GetCommentsAsync(QueryParameters queryParameters)
    {
        var query = _myDbContext.comments.AsQueryable();
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            query = query.Where(c => c.Content.Contains(queryParameters.Search));
        }
        var data = await query.OrderByDescending(c => c.CreateTime)
            .ApplyPaging(queryParameters.Page, queryParameters.PageSize)
           .ToListAsync();

        var pagination = new PaginationMetadata()
        {
            PageNumber = queryParameters.Page,
            PageSize = queryParameters.PageSize,
            TotalItemCount = await query.CountAsync()
        };
        return (data, pagination);
    }

    public Task<List<CommentRes>> GetAllCommentsAsync()
    {
        var data = _myDbContext.comments
            .Select(c => new CommentRes
            {
                Content = c.Content,
                CreateTime = c.CreateTime,
            })
            .OrderByDescending(c => c.CreateTime).ToListAsync();
        return data;
    }
}