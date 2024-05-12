using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.RecipeVote;
using receptai.api.Interfaces;
using receptai.api.Mappers;
using receptai.api.Repositories;
using receptai.data;

namespace receptai.api.Controllers;

[Route("api/vote")]
[ApiController]
public class RecipeVoteController : ControllerBase
{
    private readonly IRecipeVoteRepository _recipeVoteRepository;
    private readonly IRecipeRepository _recipeRepository;

    public RecipeVoteController(
        IRecipeVoteRepository recipeVoteRepository,
        IRecipeRepository recipeRepository)
    {
        _recipeVoteRepository = recipeVoteRepository;
        _recipeRepository = recipeRepository;
    }

    [HttpGet("{userId}/{recipeId}")]
    public async Task<IActionResult> GetByUserAndRecipeId(
        [FromRoute] int userId, [FromRoute] int recipeId)
    {
        var recipeVote = await _recipeVoteRepository.GetByUserAndRecipeId(
            userId, recipeId);

        if (recipeVote is null)
        {
            return NotFound();
        }

        return Ok(recipeVote.ToRecipeVoteDto());
    }

    [HttpGet("{recipeId}")]
    public async Task<IActionResult> GetByRecipeId([FromRoute] int recipeId)
    {
        var recipe = await _recipeRepository.GetByIdAsync(recipeId);

        if (recipe is null)
        {
            return NotFound();
        }

        var recipeVotes = await _recipeVoteRepository.GetByRecipeId(recipeId);

        return Ok(recipeVotes.Select(rv => rv.ToRecipeVoteDto()).ToList());
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRecipeVoteRequestDto recipeVoteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recipeVoteModel = recipeVoteDto.ToRecipeVoteFromCreateDto();
        await _recipeVoteRepository.CreateAsync(recipeVoteModel);

        return CreatedAtAction(
            nameof(GetByUserAndRecipeId),
            new
            {
                userId = recipeVoteModel.UserId,
                recipeId = recipeVoteModel.RecipeId
            },
            recipeVoteModel.ToRecipeVoteDto());
    }

    [HttpPut]
    [Route("{userId}/{recipeId}")]
    public async Task<IActionResult> Update([FromRoute] int userId,
        [FromRoute] int recipeId, [FromBody] UpdateRecipeVoteRequestDto recipeVoteDto)
    {
        if (!ModelState.IsValid) 
        {
            return BadRequest(ModelState);
        }

        var recipeVoteModel = await _recipeVoteRepository
            .UpdateAsync(userId, recipeId, recipeVoteDto);

        if (recipeVoteModel is null)
        {
            return NotFound();
        }

        return Ok(recipeVoteModel.ToRecipeVoteDto());
    }

    [HttpDelete]
    [Route("{userId}/{recipeId}")]
    public async Task<IActionResult> Delete([FromRoute] int userId,
        [FromRoute] int recipeId)
    {
        var recipeVoteModel = await _recipeVoteRepository
            .DeleteAsync(userId, recipeId);

        if (recipeVoteModel is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
