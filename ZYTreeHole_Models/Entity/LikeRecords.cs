namespace ZYTreeHole_Models.Entity;

// 点赞记录
public class LikeRecords
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int CommentId { get; set; }
    public DateTime LikeTime { get; set; }
}