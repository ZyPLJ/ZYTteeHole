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
    private readonly TempFilterService _filter;
    public CommentController(ILogger<CommentController> logger, ICommentsService commentService, TempFilterService filter)
    {
        _logger = logger;
        _commentService = commentService;
        _filter = filter;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ApiResponse<List<CommentRes>>> GetAll()
    {
        var data = await _commentService.GetAllCommentsAsync();
        return new ApiResponse<List<CommentRes>>(data);
    }
    [HttpPost]
    public async Task<ApiResponse<ZyComments>> CreateComment(CommentDto commentDto)
    {
        //查询用户信息 ToDo: 后续需要改成获取用户ip地址
        commentDto.UserId = 1;
        
        string msg = string.Empty;
        int code = 200;
        //检查是否有敏感词
        if (_filter.CheckBadWord(commentDto.Content)) {
            commentDto.IsNeedAudit = true;
            commentDto.Visible = false;
            msg = "小管家发现您可能使用了不良用语，该评论将在审核通过后展示~";
            code = 400;
        }
        else {
            commentDto.Visible = true;
            msg = "评论由小管家审核通过，感谢您参与讨论~";
        }
        var comment = await _commentService.CreateCommentAsync(commentDto);
        _logger.LogInformation("Create comment success, commentId: {0}", comment.Id);
        return new ApiResponse<ZyComments>(comment) {
            Message = msg,
            StatusCode = code
        };
    }
}