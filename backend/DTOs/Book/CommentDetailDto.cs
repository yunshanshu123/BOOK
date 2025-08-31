public class CommentDetailDto
{
    public string? CommentID { get; set; }
    public string? ReaderID { get; set; }
    public string? ISBN { get; set; }
    public int? RATING { get; set; }
    public required string ReviewContent { get; set; }
    public DateTime CreateTime { get; set; }
    public string Status { get; set; } = "正常";
}