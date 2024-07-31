namespace ZYTreeHole_Models.ViewModels.Requests;

public class CommentDto
{
    public int? UserId { get; set; }
    public int? ParentId { get; set; }
    public string? Content { get; set; }
    public string? Email { get; set; }
    public bool IsNeedAudit { get; set; } = false;
    public bool Visible { get; set; }
}