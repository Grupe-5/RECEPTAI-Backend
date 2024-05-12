using api.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptai.data;

namespace receptai.api.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, IImageService imageService) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IImageService _imageService = imageService;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".bmp"];
    private readonly string[] _allowedContentTypes = ["image/jpeg", "image/png", "image/bmp"];

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var createdUser = await _userManager.CreateAsync(user, registerDto.Password!);
            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = user.UserName!,
                            Email = user.Email!,
                            Token = _tokenService.CreateToken(user),
                        }
                    );
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
        {
            return Unauthorized("Invalid login");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid login");
        }

        return Ok(
            new NewUserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user)
            }
        );
    }

    [HttpDelete("delete_account")]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) {
            return NotFound();
        }

        var res = await _userManager.DeleteAsync(user);
        if (res.Succeeded) {
            return Ok();
        } else {
            return BadRequest(res.Errors);
        }
    }

    [HttpGet("info/me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        /* If some private data is required, this endpoint can return more than generic one */
        return Ok(user.ToUserInfoDto());
    }

    [HttpGet("info/{id:int}")]
    public async Task<IActionResult> GetUserInfo([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.ToUserInfoDto());
    }

    [HttpPost("img")]
    [Authorize]
    public async Task<IActionResult> UploadUserImg(IFormFile file)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(fileExtension) || !_allowedContentTypes.Contains(file.ContentType))
        {
            return BadRequest("Invalid file type. Only image files are allowed.");
        }

        try {
            ImageDimensions dimensions = new()
            {
                Width = 256,
                Height = 256,
                KeepAspectRatio = false
            };

            int id = await _imageService.StoreImageAsync(file.OpenReadStream(), dimensions);

            /* Check if user already has image */
            if (user.ImgId != null)
            {
                await _imageService.DeleteImageAsync(user.ImgId.Value);
            }

            user.ImgId = id;
            await _userManager.UpdateAsync(user);

            return Ok(id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpDelete("img")]
    [Authorize]
    public async Task<IActionResult> DeleteUserImg()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        if (user.ImgId != null)
        {
            /* Deleting "cascades" foreign key in user to null automatically */
            await _imageService.DeleteImageAsync(user.ImgId.Value);
        }
        return Ok();
    }
}
