using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using receptai.api.Dtos.CommentVote;
using receptai.api.Extensions;
using receptai.api.Interfaces;
using receptai.api.Mappers;

namespace receptai.api.Controllers;

[Route("api/comment_vote")]
[ApiController]
public class CommentVoteController : ControllerBase
{
    private readonly ICommentVoteRepository _commentVoteRepository;
    private readonly ICommentRepository _commentRepository;

    public CommentVoteController(
        ICommentVoteRepository commentVoteRepository,
        ICommentRepository commentRepository)
    {
        _commentVoteRepository = commentVoteRepository;
        _commentRepository = commentRepository;
    }

    [HttpGet("me/{commentId}")]
    [Authorize]
    public async Task<IActionResult> GetByUserAndCommentId([FromRoute] int commentId)
    {
        var commentVote = await _commentVoteRepository.GetByUserAndCommentId(
            User.GetId(), commentId);

        if (commentVote is null)
        {
            return NotFound();
        }

        return Ok(commentVote.ToCommentVoteDto());
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetByCommentId([FromRoute] int commentId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);

        if (comment is null)
        {
            return NotFound();
        }

        var commentVotes = await _commentVoteRepository.GetByCommentId(commentId);

        return Ok(commentVotes.Select(cv => cv.ToCommentVoteDto()).ToList());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateCommentVoteRequestDto commentVoteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetId();
        var existingVote = await _commentVoteRepository.GetByUserAndCommentId(userId, commentVoteDto.CommentId);

        if (existingVote != null)
        {
            return Conflict(new { message = "User has already voted on this comment." });
        }

        var commentVoteModel = commentVoteDto.ToCommentVoteFromCreateDto();
        commentVoteModel.UserId = userId;
        await _commentVoteRepository.CreateAsync(commentVoteModel);

        return CreatedAtAction(
            nameof(GetByUserAndCommentId),
            new
            {
                userId = commentVoteModel.UserId,
                commentId = commentVoteModel.CommentId
            },
            commentVoteModel.ToCommentVoteDto());
    }

    [HttpPut]
    [Route("{commentId}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] int commentId,
        [FromBody] UpdateCommentVoteRequestDto commentVoteDto)
    {
        if (!ModelState.IsValid) 
        {
            return BadRequest(ModelState);
        }

        var commentVoteModel = await _commentVoteRepository
            .UpdateAsync(User.GetId(), commentId, commentVoteDto);

        if (commentVoteModel is null)
        {
            return NotFound();
        }

        return Ok(commentVoteModel.ToCommentVoteDto());
    }

    [HttpDelete]
    [Route("{commentId}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int commentId)
    {
        var commentVoteModel = await _commentVoteRepository
            .DeleteAsync(User.GetId(), commentId);

        if (commentVoteModel is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
