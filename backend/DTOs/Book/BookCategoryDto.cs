
public class Category
{
    public string CategoryID { get; set; } = string.Empty;
    public required string CategoryName { get; set; }
    public string? ParentCategoryID { get; set; }
}