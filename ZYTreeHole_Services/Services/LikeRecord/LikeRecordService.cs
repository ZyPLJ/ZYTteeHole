using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZYTreeHole_Models;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole_Models.ViewModels.Responses;
using ZYTreeHole_Share;


namespace ZYTreeHole_Services.Services.LikeRecord;

public class LikeRecordService: ILikeRecordService
{
    private readonly MyDbContext  _myDbContext;

    public LikeRecordService(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }
    public async Task<List<RecordsRes>> GetRecordsAsync(QueryParameters? queryParameters)
    {
        var query = _myDbContext.likeRecords.AsQueryable();

        var data = await query
            .GroupBy(x => x.CommentId)
            .Select(x => new RecordsRes()
            {
                CommentId = x.Key,
                Count = x.Count()
            }).ToListAsync();
        
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            // 是否升序
            var isAscending = !queryParameters.SortBy.StartsWith("-");
            var orderByProperty = queryParameters.SortBy.Trim('-');

            // 对分组后的数据进行排序  
            data = isAscending   
                ? data.OrderBy(x => x.GetType().GetProperty(orderByProperty)?.GetValue(x, null)).ToList()  
                : data.OrderByDescending(x => x.GetType().GetProperty(orderByProperty)?.GetValue(x, null)).ToList(); 
        }
        
        return data;
    }

    public async Task<bool> LikeAsync(LikeRecordRequest request)
    {
        await _myDbContext.likeRecords.AddAsync(new LikeRecords()
        {
            CommentId = request.CommentId,
            UserId = request.UserId,
            LikeTime = DateTime.Now
        });
        return await _myDbContext.SaveChangesAsync() > 0;
    }
    
}