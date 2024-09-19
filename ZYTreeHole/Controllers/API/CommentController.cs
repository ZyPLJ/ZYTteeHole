using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole_Models.ViewModels.Responses;
using ZYTreeHole_Services.Services;

namespace ZYTreeHole.Controllers.API;


[ApiController]
[Route("Api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentsService _commentService;
    public CommentController(ICommentsService commentService)
    {
        _commentService = commentService;
    }
    [HttpGet]
    public async Task<ApiResponsePaged<CommentApiRes>> GetComments([FromQuery] QueryParameters queryParameters)
    {
        var (data,meta) = await _commentService.GetCommentsAsync(queryParameters);
        return new ApiResponsePaged<CommentApiRes>(data,meta);
    }
    /// <summary>
    /// 审核通过
    /// </summary>
    [Authorize]
    [HttpPost("{id:int}/[action]")]
    public async Task<ApiResponse<ZyComments>> Accept([FromRoute] int id, [FromBody] CommentAcceptDto dto) {
        var item = await _commentService.GetByIdAsync(id);
        if (item == null) return ApiResponse.NotFound();
        return new ApiResponse<ZyComments>(await _commentService.Accept(item, dto.Reason));
    }
    /// <summary>
    /// 审核拒绝
    /// </summary>
    [Authorize]
    [HttpPost("{id:int}/[action]")]
    public async Task<ApiResponse<ZyComments>> Reject([FromRoute] int id, [FromBody] CommentRejectDto dto) {
        var item = await _commentService.GetByIdAsync(id);
        if (item == null) return ApiResponse.NotFound();
        return new ApiResponse<ZyComments>(await _commentService.Reject(item, dto.Reason));
    }
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ApiResponse> DeleteComment(int id)
    {
        var comment = await _commentService.GetByIdAsync(id);
        if (comment == null) return ApiResponse.NotFound();
        var result = await _commentService.DeleteCommentAsync(comment);
        if (!result) return new ApiResponse(){StatusCode = 500,Message = "删除失败"};
        return new ApiResponse(){Message = "删除成功"};
    }
}