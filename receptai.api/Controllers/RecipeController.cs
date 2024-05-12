using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Recipe;
using receptai.api.Extensions;
using receptai.api.Helpers;
using receptai.api.Interfaces;
using receptai.api.Mappers;

namespace receptai.api.Controllers;

[Route("api/recipe")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeVoteRepository _recipeVoteRepository;
    private readonly ISubfoodditRepository _subfoodditRepository;

    public RecipeController(IRecipeRepository recipeRepository,
        IRecipeVoteRepository recipeVoteRepository,
        ISubfoodditRepository subfoodditRepository)
    {
        _recipeRepository = recipeRepository;
        _recipeVoteRepository = recipeVoteRepository;
        _subfoodditRepository = subfoodditRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var recipes = await _recipeRepository.GetAllAsync();
        var recipeDto = recipes.Select(r => r.ToRecipeDto());

        return Ok(recipeDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var recipe = await _recipeRepository.GetByIdAsync(id);

        if (recipe is null)
        {
            return NotFound();
        }

        return Ok(recipe.ToRecipeDto());
    }

    [HttpGet("{id:int}/aggregated_votes")]
    public async Task<IActionResult> GetAggregatedVotesById(
        [FromRoute] int id)
    {
        var recipeVotes = await _recipeVoteRepository
            .GetAggregatedRecipeVotesByRecipeId(id);

        return Ok(recipeVotes);
    }

    [HttpGet("by_user/{userId}")]
    public async Task<IActionResult> GetRecipesByUserId(int userId)
    {
        var recipes = await _recipeRepository.GetRecipesByUserId(userId);

        return Ok(recipes);
    }

    [HttpGet("by_subfooddit/{subfoodditId}")]
    public async Task<IActionResult> GetRecipesBySubfoodditId(int subfoodditId)
    {
        var recipes = await _recipeRepository.GetRecipesBySubfoodditId(subfoodditId);

        return Ok(recipes);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRecipeRequestDto recipeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recipeModel = recipeDto.ToRecipeFromCreateDto();

        var subfoodditName = _subfoodditRepository
            .GetByIdAsync(recipeModel.SubfoodditId).Result?.Title;

        if (subfoodditName is not null)
        {
            recipeModel.SubfoodditName = subfoodditName;
        }

        recipeModel.UserName = User.GetUsername();

        await _recipeRepository.CreateAsync(recipeModel);

        return CreatedAtAction(nameof(GetById),
            new { id = recipeModel.RecipeId },
            recipeModel.ToRecipeDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] UpdateRecipeRequestDto recipeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recipeModel = await _recipeRepository
            .UpdateAsync(id, recipeDto);

        if (recipeModel is null)
        {
            return NotFound();
        }

        return Ok(recipeModel.ToRecipeDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var recipeModel = await _recipeRepository.DeleteAsync(id);

        if (recipeModel is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
