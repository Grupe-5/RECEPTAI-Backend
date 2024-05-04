using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Recipe;
using receptai.api.Interfaces;
using receptai.api.Mappers;
using receptai.data;

namespace receptai.api.Controllers;

[Route("api/recipe")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly RecipePlatformDbContext _context;
    private readonly IRecipeRepository _recipeRepository;

    public RecipeController(RecipePlatformDbContext context,
        IRecipeRepository recipeRepository)
    {
        _context = context;
        _recipeRepository = recipeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? userId,
        [FromQuery] int? subfoodditId)
    {
        var recipes = await _recipeRepository.GetAllAsync();
        var recipeDto = recipes.Select(r => r.ToRecipeDto());

        if (userId.HasValue)
        {
            recipeDto = recipeDto
                .Where(r => r.UserId == userId.Value);
        }

        if (subfoodditId.HasValue)
        {
            recipeDto = recipeDto
                .Where(r => r.SubfoodditId == subfoodditId.Value);
        }

        var filteredRecipes = recipeDto.ToList();

        if(filteredRecipes.Count == 0)
        {
            return NotFound($"No recipes found matching the given filters.");
        }

        return Ok(recipeDto);
    }

    [HttpGet("{id}")]
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
        var recipeModel = recipeDto.ToRecipeFromCreateDto();
        await _recipeRepository.CreateAsync(recipeModel);

        return CreatedAtAction(nameof(GetById),
            new { id = recipeModel.RecipeId },
            recipeModel.ToRecipeDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] UpdateRecipeRequestDto recipeDto)
    {
        var recipeModel = await _recipeRepository
            .UpdateAsync(id, recipeDto);

        if (recipeModel is null)
        {
            return NotFound();
        }

        return Ok(recipeModel.ToRecipeDto());
    }

    [HttpDelete]
    [Route("{id}")]
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
