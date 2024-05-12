using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Comment;
using receptai.api.Helpers;
using receptai.api.Interfaces;
using receptai.api.Mappers;
using receptai.api.Repositories;
using receptai.data;

namespace receptai.api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet("by_user/{userId}")]
    public async Task<IActionResult> GetCommentsByUserId(int userId)
    {
        var comments = await _commentRepository.GetCommentsByUserId(userId);
        if (comments == null || comments.Count == 0)
        {
            return NotFound("No comments found for the provided user ID.");
        }
        return Ok(comments);
    }

    [HttpGet("by_recipe/{recipeId}")]
    public async Task<IActionResult> GetCommentsByRecipeId(int recipeId)
    {
        var comments = await _commentRepository.GetCommentsByRecipeId(recipeId);
        if (comments == null || comments.Count == 0)
        {
            return NotFound("No comments found for the provided recipe ID.");
        }
        return Ok(comments);
    } 

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment is null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCommentRequestDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var commentModel = commentDto.ToCommentFromCreateDto();
        await _commentRepository.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById),
            new { id = commentModel.CommentId },
            commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] UpdateCommentRequestDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var commentModel = await _commentRepository
            .UpdateAsync(id, commentDto);

        if (commentModel is null)
        {
            return NotFound();
        }

        return Ok(commentModel.ToCommentDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var commentModel = await _commentRepository.DeleteAsync(id);

        if (commentModel is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
