﻿using CodeLab.Share.ViewModels;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole_Models.ViewModels.Responses;

namespace ZYTreeHole_Services.Services;

public interface ICommentsService
{
       /// <summary>
       /// 创建评论
       /// </summary>
       /// <param name="commentDto"></param>
       /// <returns></returns>
       Task<ZyComments> CreateCommentAsync(CommentDto commentDto);
       /// <summary>
       /// 通过审核
       /// </summary>
       /// <param name="comment"></param>
       /// <param name="reason"></param>
       /// <returns></returns>
       Task<ZyComments> Accept(ZyComments comment, string? reason = null);
       /// <summary>
       /// 拒绝审核
       /// </summary>
       /// <param name="comment"></param>
       /// <param name="reason"></param>
       /// <returns></returns>
       Task<ZyComments> Reject(ZyComments comment, string reason);
       Task<(List<CommentApiRes>,PaginationMetadata)> GetCommentsAsync(QueryParameters queryParameters);
       Task<List<CommentRes>> GetAllCommentsAsync();
       Task<ZyComments?> GetByIdAsync(int id);
       Task<bool> DeleteCommentAsync(ZyComments comment);
       Task<List<CommentRes>> GetRankingCommentsAsync(QueryParameters? queryParameters);
       Task<List<ZyComments>> GetCommentsByIdsAsync(List<int> ids);
       Task<bool> DeleteCommentsAsync(List<ZyComments> comments);
}