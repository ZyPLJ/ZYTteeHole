using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole_Models.ViewModels.Responses;
using ZYTreeHole_Services.Services;
using ZYTreeHole_Services.Services.User;

namespace ZYTreeHole.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentsService _commentService;
    private readonly IUsersService _usersService;
    private readonly TempFilterService _filter;

    public CommentController(ILogger<CommentController> logger, ICommentsService commentService,
        IUsersService usersService,TempFilterService filter)
    {
        _logger = logger;
        _commentService = commentService;
        _usersService = usersService;
        _filter = filter;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ApiResponse<List<CommentRes>>> GetAll()
    {
        var data = await _commentService.GetAllCommentsAsync();
        return new ApiResponse<List<CommentRes>>(data);
    }
    [HttpGet]
    public async Task<ApiResponsePaged<ZyComments>> GetComments([FromQuery] QueryParameters queryParameters)
    {
        var (data,meta) = await _commentService.GetCommentsAsync(queryParameters);
        return new ApiResponsePaged<ZyComments>(data,meta);
    }
    [HttpPost]
    public async Task<ApiResponse<ZyComments>> CreateComment(CommentDto commentDto)
    {
        //查询用户信息
        UserDto userDto = new UserDto()
        {
            Email = commentDto.Email ?? "",
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString().Split(":")?.Last(),
        };
        var user = await _usersService.GetUserOrAddAsync(userDto);
        commentDto.UserId = user.Id;
        
        string msg = string.Empty;
        //检查是否有敏感词
        if (_filter.CheckBadWord(commentDto.Content)) {
            commentDto.IsNeedAudit = true;
            commentDto.Visible = false;
            msg = "小管家发现您可能使用了不良用语，该评论将在审核通过后展示~";
        }
        else {
            commentDto.Visible = true;
            msg = "评论由小管家审核通过，感谢您参与讨论~";
        }
        var comment = await _commentService.CreateCommentAsync(commentDto);
        _logger.LogInformation("Create comment success, commentId: {0}", comment.Id);
        return new ApiResponse<ZyComments>(comment) {
            Message = msg
        };
    }
    /// <summary>
    /// 审核通过
    /// </summary>
    [HttpPost("{id:int}/[action]")]
    public async Task<ApiResponse<ZyComments>> Accept([FromRoute] int id, [FromBody] CommentAcceptDto dto) {
        var item = await _commentService.GetByIdAsync(id);
        if (item == null) return ApiResponse.NotFound();
        return new ApiResponse<ZyComments>(await _commentService.Accept(item, dto.Reason));
    }

    /// <summary>
    /// 审核拒绝
    /// </summary>
    [HttpPost("{id:int}/[action]")]
    public async Task<ApiResponse<ZyComments>> Reject([FromRoute] int id, [FromBody] CommentRejectDto dto) {
        var item = await _commentService.GetByIdAsync(id);
        if (item == null) return ApiResponse.NotFound();
        return new ApiResponse<ZyComments>(await _commentService.Reject(item, dto.Reason));
    }
    
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