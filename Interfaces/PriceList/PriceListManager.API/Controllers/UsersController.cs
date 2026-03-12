using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceListManager.API.Models.DTOs;
using PriceListManager.API.Services;

namespace PriceListManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("search")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> Search([FromQuery] string query)
    {
        var users = await _userService.SearchAsync(query);
        return Ok(users);
    }

    [HttpPost("admin")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> CreateAdmin([FromBody] CreateAdminRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Email обязателен" });
        }

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var (user, error, tempPassword) = await _userService.CreateAdminAsync(request, currentUserId);

        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return CreatedAtAction(nameof(GetById), new { id = user!.Id }, new 
        { 
            user, 
            tempPassword,
            message = $"Пользователь создан. Временный пароль: {tempPassword}"
        });
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
    {
        var (success, error) = await _userService.UpdateRoleAsync(id, request.Role);
        
        if (!success)
        {
            return BadRequest(new { message = error });
        }

        return NoContent();
    }
}
