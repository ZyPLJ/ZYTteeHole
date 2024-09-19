namespace ZYTreeHole_Models.ViewModels.Responses;

public class CommentApiRes
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? ParentId { get; set; }
    public string Content { get; set; }
    public DateTime CreateTime { get; set; }
    public bool Visible { get; set; }
    public bool IsNeedAudit { get; set; } = false;
    public string? Reason { get; set; }
    public int LikeCount { get; set; }
}