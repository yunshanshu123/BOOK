using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("search")]
    public async Task<IEnumerable<CommentDetailDto>> Search(string ISBN)
    {
        return await _commentService.SearchCommentAsync(ISBN ?? "");
    }

    
    [HttpPost("add")]
    public async Task<ActionResult> AddComment([FromBody] CommentDetailDto commentDto)
    {
        var result = await _commentService.AddCommentAsync(commentDto);
        if (result > 0)
        {
            return Ok(new { Message = "评论添加成功" });
        }
        return BadRequest(new { Message = "评论添加失败" });
    }
}