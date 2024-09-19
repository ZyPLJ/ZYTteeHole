using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole_Models.ViewModels.Responses;

namespace ZYTreeHole_Services.Services.LikeRecord;

public interface ILikeRecordService
{
    Task<List<RecordsRes>> GetRecordsAsync(QueryParameters? queryParameters);
    Task<bool> LikeAsync(LikeRecordRequest request);
}