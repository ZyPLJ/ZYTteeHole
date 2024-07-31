namespace ZYTreeHole_Models.Entity;

public class ZyComments
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? ParentId { get; set; }
    public string Content { get; set; }
    public DateTime CreateTime { get; set; }
    
    public bool Visible { get; set; }

    /// <summary>
    /// 是否需要审核
    /// </summary>
    public bool IsNeedAudit { get; set; } = false;
    /// <summary>
    /// 原因
    /// <para>如果验证不通过的话，可能会附上原因</para>
    /// </summary>
    public string? Reason { get; set; }
}