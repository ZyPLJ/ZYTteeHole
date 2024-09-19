using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole_Models.ViewModels.Responses;
using ZYTreeHole_Services.Services.LikeRecord;

namespace ZYTreeHole.Controllers;

[Route("[controller]")]
public class LikeRecordController : ControllerBase
{
    private readonly ILikeRecordService _likeRecordService;

    public LikeRecordController(ILikeRecordService likeRecordService)
    {
        _likeRecordService = likeRecordService;
    }

    // GET: 获取点赞数据
    [HttpGet]
    public async Task<ApiResponse<List<RecordsRes>>> Get(QueryParameters? queryParameters)
    {
        var records = await _likeRecordService.GetRecordsAsync(queryParameters);
        return new ApiResponse<List<RecordsRes>>(records)
        {
            Message = "获取点赞数据成功",
            StatusCode = 200
        };
    }
    // POST: 新增点赞数据
    [HttpPost]
    public async Task<ApiResponse> Post([FromBody] LikeRecordRequest request)
    {
        var result = await _likeRecordService.LikeAsync(request);
        if (result)
            return new ApiResponse() { Message = "点赞成功", StatusCode = 200 };
        return new ApiResponse() { Message = "点赞失败", StatusCode = 500 };
    }
}