using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Recipe;
using receptai.api.Helpers;
using receptai.api.Interfaces;
using receptai.api.Mappers;
using receptai.data;

namespace receptai.api.Controllers;

[Route("api/recipe")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipeController(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] QueryRecipe query)
    {
        var recipes = await _recipeRepository.GetAllAsync(query);
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

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRecipeRequestDto recipeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recipeModel = recipeDto.ToRecipeFromCreateDto();
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
