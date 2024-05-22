using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using receptai.api.Dtos.Subfooddit;
using receptai.api.Extensions;
using receptai.api.Interfaces;
using receptai.api.Mappers;

namespace receptai.api.Controllers;

[Route("api/subfooddit")]
[ApiController]
public class SubfoodditController(ISubfoodditRepository subfoodditRepository) : ControllerBase
{
    private readonly ISubfoodditRepository _subfoodditRepository = subfoodditRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subfooddits = await _subfoodditRepository.GetAllAsync();
        var subfoodditDto = subfooddits.Select(s => s.ToSubfoodditDto());

        return Ok(subfoodditDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var subfooddit = await _subfoodditRepository.GetByIdAsync(id);

        if (subfooddit is null)
        {
            return NotFound();
        }
        return Ok(subfooddit.ToSubfoodditDto());
    }

    [HttpGet("by_user/{userId}")]
    public async Task<IActionResult> GetSubfoodditsByUserId(int userId)
    {
        var subfooddits = await _subfoodditRepository.GetSubfoodditsByUserId(userId);
        if (subfooddits == null || subfooddits.Count == 0)
        {
            return NotFound("No subfooddits found for the provided user ID.");
        }
        return Ok(subfooddits);
    }

    [HttpGet("by_subfooddit/{subfoodditId}")]
    public async Task<IActionResult> GetUsersBySubfoodditId(int subfoodditId)
    {
        var userSummaries = await _subfoodditRepository.GetUsersBySubfoodditId(subfoodditId);
        if (userSummaries == null || userSummaries.Count == 0)
        {
            return NotFound("No users found for the provided subfooddit ID.");
        }
        return Ok(userSummaries);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSubfoodditRequestDto subfoodditDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var subfoodditModel = subfoodditDto.ToSubfoodditFromCreateDto();
        await _subfoodditRepository.CreateAsync(subfoodditModel);

        return CreatedAtAction(nameof(GetById),
            new { id = subfoodditModel.SubfoodditId },
            subfoodditModel.ToSubfoodditDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] UpdateSubfoodditRequestDto subfoodditDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var subfoodditModel = await _subfoodditRepository
            .UpdateAsync(id, subfoodditDto);
            
        if (subfoodditModel is null)
        {
            return NotFound();
        }
        return Ok(subfoodditModel.ToSubfoodditDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var subfoodditModel = await _subfoodditRepository.DeleteAsync(id);

        if (subfoodditModel is null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("add_user")]
    [Authorize]
    public async Task<IActionResult> AddUser(int subfoodditId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetId();
        var success = await _subfoodditRepository.AddUserToSubfooddit(subfoodditId, userId);
        if (!success)
        {
            return NotFound("Subfooddit/user not found, or already added");
        }
        return Ok();
    }

    [HttpDelete("remove_user")]
    [Authorize]
    public async Task<IActionResult> RemoveUser(int subfoodditId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetId();
        var success = await _subfoodditRepository.RemoveUserFromSubfooddit(subfoodditId, userId);
        if (!success)
        {
            return NotFound("Subfooddit/user not found, or not present");
        }
        return NoContent();
    }
}
