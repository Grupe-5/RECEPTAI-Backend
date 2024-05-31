using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using receptai.api.Dtos.Comment;
using receptai.api.Extensions;
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
    private readonly ICommentVoteRepository _commentVoteRepository;

    public CommentController(ICommentRepository commentRepository,
        ICommentVoteRepository commentVoteRepository)
    {
        _commentRepository = commentRepository;
        _commentVoteRepository = commentVoteRepository;
    }

    private async Task<VoteType?> GetVoteInfo(int commentId)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return null;
        }

        var commentVote = await _commentVoteRepository.GetByUserAndCommentId(User.GetId(), commentId);
        return commentVote?.VoteType;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment is null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto(await GetVoteInfo(comment.CommentId)));
    }

    [HttpGet("by_user/{userId}")]
    public async Task<IActionResult> GetCommentsByUserId(int userId)
    {
        var comments = await _commentRepository.GetCommentsByUserId(userId);
        var commentDtos = await Task.WhenAll(comments.Select(async comment =>
        {
            var voteInfo = await GetVoteInfo(comment.CommentId);
            return comment.ToCommentDto(voteInfo);
        }));
        return Ok(commentDtos);
    }

    [HttpGet("by_recipe/{recipeId}")]
    public async Task<IActionResult> GetCommentsByRecipeId(int recipeId)
    {
        var comments = await _commentRepository.GetCommentsByRecipeId(recipeId);
        var commentDtos = await Task.WhenAll(comments.Select(async comment =>
        {
            var voteInfo = await GetVoteInfo(comment.CommentId);
            return comment.ToCommentDto(voteInfo);
        }));
        return Ok(commentDtos);
    }

    [HttpGet("aggregated_votes/{id:int}")]
    public async Task<IActionResult> GetAggregatedVotesById(
    [FromRoute] int id)
    {
        var commentVotes = await _commentVoteRepository
            .GetAggregatedCommentVotesByCommentId(id);

        return Ok(commentVotes);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateCommentRequestDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var commentModel = commentDto.ToCommentFromCreateDto();

        commentModel.UserId = User.GetId();
        commentModel.Version = Guid.NewGuid();

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

        Comment? commentModel;

        try
        {
            commentModel = await _commentRepository
                .UpdateAsync(id, commentDto);
        }
        catch
        {
            return Conflict("A concurrency conflict while updating the comment has occured.");
        }
        
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
