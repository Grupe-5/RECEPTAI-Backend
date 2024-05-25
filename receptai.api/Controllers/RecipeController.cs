using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using receptai.api.Dtos.Recipe;
using receptai.api.Extensions;
using receptai.api.Interfaces;
using receptai.api.Mappers;
using receptai.data;

namespace receptai.api.Controllers;

[Route("api/recipe")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeVoteRepository _recipeVoteRepository;
    private readonly ISubfoodditRepository _subfoodditRepository;
    private readonly IImageService _imageService;

    public RecipeController(IRecipeRepository recipeRepository,
        IRecipeVoteRepository recipeVoteRepository,
        ISubfoodditRepository subfoodditRepository,
        IImageService imageService)
    {
        _recipeRepository = recipeRepository;
        _recipeVoteRepository = recipeVoteRepository;
        _subfoodditRepository = subfoodditRepository;
        _imageService = imageService;
    }

    private async Task<VoteType?> GetVoteInfo(int recipeId) {
        if (!(User.Identity?.IsAuthenticated ?? false)) {
            return null;
        }

        var commentVote = await _recipeVoteRepository.GetByUserAndRecipeId(User.GetId(), recipeId);
        return commentVote?.VoteType;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var recipes = await _recipeRepository.GetAllAsync(offset, limit);
        var recipeDto = await Task.WhenAll(recipes.Select(async recipe => 
        {
            var voteInfo = await GetVoteInfo(recipe.RecipeId);
            return recipe.ToRecipeDto(voteInfo);
        }));

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

        return Ok(recipe.ToRecipeDto(await GetVoteInfo(recipe.RecipeId)));
    }

    [HttpGet("by_user/{userId}")]
    public async Task<IActionResult> GetRecipesByUserId(int userId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var recipes = await _recipeRepository.GetRecipesByUserId(userId, offset, limit);
        var recipeDto = await Task.WhenAll(recipes.Select(async recipe => 
        {
            var voteInfo = await GetVoteInfo(recipe.RecipeId);
            return recipe.ToRecipeDto(voteInfo);
        }));
        return Ok(recipeDto);
    }

    [HttpGet("by_subfooddit/{subfoodditId}")]
    public async Task<IActionResult> GetRecipesBySubfoodditId(int subfoodditId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var recipes = await _recipeRepository.GetRecipesBySubfoodditId(subfoodditId, offset, limit);
        var recipeDto = await Task.WhenAll(recipes.Select(async recipe => 
        {
            var voteInfo = await GetVoteInfo(recipe.RecipeId);
            return recipe.ToRecipeDto(voteInfo);
        }));
        return Ok(recipeDto);
    }

    [HttpGet("aggregated_votes/{id:int}")]
    public async Task<IActionResult> GetAggregatedVotesById(
    [FromRoute] int id)
    {
        var recipeVotes = await _recipeVoteRepository
            .GetAggregatedRecipeVotesByRecipeId(id);

        return Ok(recipeVotes);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromForm] CreateRecipeRequestDto recipeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recipeModel = recipeDto.ToRecipeFromCreateDto();

        var subfoodditName = _subfoodditRepository
            .GetByIdAsync(recipeModel.SubfoodditId).Result?.Title;
        if (subfoodditName == null) {
            return BadRequest("Invalid subfooddit");
        }

        recipeModel.SubfoodditName = subfoodditName!;

        if (recipeDto.Photo != null && recipeDto.Photo.Length != 0)
        {
            try {
                ImageDimensions dimensions = new()
                {
                    Width = 1024,
                    KeepAspectRatio = true
                };

                recipeModel.ImgId = await _imageService.StoreImageAsync(recipeDto.Photo.OpenReadStream(), dimensions);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        recipeModel.UserId = User.GetId();
        recipeModel.UserName = User.GetUsername();

        try {
            await _recipeRepository.CreateAsync(recipeModel);

            return CreatedAtAction(nameof(GetById),
                new { id = recipeModel.RecipeId },
                recipeModel.ToRecipeDto());
        }
        catch (Exception e)
        {
            if (recipeModel.ImgId != null)
            {
                await _imageService.DeleteImageAsync(recipeModel.ImgId.Value);
            }
            return StatusCode(500, e);
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromForm] UpdateRecipeRequestDto recipeDto, [FromQuery] bool remove_photo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int? imageId = null;
        if (recipeDto.Photo != null && recipeDto.Photo.Length != 0 && remove_photo == false)
        {
            try {
                ImageDimensions dimensions = new()
                {
                    Width = 1024,
                    KeepAspectRatio = true
                };

                imageId = await _imageService.StoreImageAsync(recipeDto.Photo.OpenReadStream(), dimensions);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        var recipeModel = await _recipeRepository
            .UpdateAsync(id, recipeDto, imageId, remove_photo);

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
