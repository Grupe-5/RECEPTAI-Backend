using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Subfooddit;
using receptai.api.Helpers;
using receptai.api.Interfaces;
using receptai.api.Mappers;
using receptai.api.Repositories;
using receptai.data;

namespace receptai.api.Controllers;

[Route("api/subfooddit")]
[ApiController]
public class SubfoodditController : ControllerBase
{
    private readonly ISubfoodditRepository _subfoodditRepository;

    public SubfoodditController(ISubfoodditRepository subfoodditRepository)
    {
        _subfoodditRepository = subfoodditRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] QuerySubfooddit query)
    {
        var subfooddits = await _subfoodditRepository.GetAllAsync(query);
        var subfoodditDto = subfooddits.Select(r => r.ToSubfoodditDto());

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
}

