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
        var data = await _myDbContext.comments.FindAsync(comment.Id);
        data.Visible = true;
        data.IsNeedAudit = true;
        data.Reason = reason;
        await _myDbContext.SaveChangesAsync();
        return comment;
    }
    public async Task<ZyComments> Reject(ZyComments comment, string reason) {
        var data = await _myDbContext.comments.FindAsync(comment.Id);
        data.Visible = false;
        data.IsNeedAudit = false;
        data.Reason = reason;
        await _myDbContext.SaveChangesAsync();
        return comment;
    }

    public async Task<(List<CommentApiRes>,PaginationMetadata)> GetCommentsAsync(QueryParameters queryParameters)
    {
        var query = _myDbContext.comments.AsQueryable();
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            query = query.Where(c => c.Content.Contains(queryParameters.Search));
        }
        //关联点赞表
        var data = await query
            .OrderByDescending(c => c.CreateTime)
            .Select(c => new CommentApiRes
            {
                Id = c.Id,
                Content = c.Content,
                CreateTime = c.CreateTime,
                UserId = c.UserId,
                ParentId = c.ParentId,
                Visible = c.Visible,
                IsNeedAudit = c.IsNeedAudit,
                Reason = c.Reason,
                LikeCount = _myDbContext.likeRecords.Count(lr => lr.CommentId == c.Id) 
            })
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

    public async Task<List<CommentRes>> GetAllCommentsAsync()
    {
        var data =await _myDbContext.comments
            .Where(c => c.Visible)
            .Select(c => new CommentRes
            {
                Id = c.Id,
                Content = c.Content,
                CreateTime = c.CreateTime,
            })
            .OrderByDescending(c => c.CreateTime).ToListAsync();
        return data;
    }

    public async Task<ZyComments?> GetByIdAsync(int id)
    {
        return await _myDbContext.comments.FindAsync(id);
    }

    public async Task<bool> DeleteCommentAsync(ZyComments comment)
    {
        _myDbContext.comments.Remove(comment);
        return await _myDbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<CommentRes>> GetRankingCommentsAsync(QueryParameters? queryParameters)
    {
        var data = await _myDbContext.comments
            .OrderByDescending(c => c.CreateTime)
            .Select(c => new CommentRes
            {
                Id = c.Id,
                Content = c.Content,
                CreateTime = c.CreateTime,
                LikeCount = _myDbContext.likeRecords.Count(lr => lr.CommentId == c.Id) 
            })
            .OrderByDescending(c => c.LikeCount)
            .ToListAsync();
        return data;
    }
}