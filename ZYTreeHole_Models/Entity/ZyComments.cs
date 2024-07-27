namespace ZYTreeHole_Models.Entity;

public class ZyComments
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ParentId { get; set; }
    public string? Content { get; set; }
    public DateTime CreateTime { get; set; }
}