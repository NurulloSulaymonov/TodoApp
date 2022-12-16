using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Data.Dtos;
using WebApi.Infrastructure.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class AccountController:ControllerBase
{
    private readonly UserService _accountService;

    public AccountController(UserService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        
        var result= await _accountService.Login(loginDto);
        if(result == null) return BadRequest(result);
        
        return Ok(result);
    }
    
    //register
    [HttpPost("register")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (ModelState.IsValid == false) return BadRequest(registerDto);
        var result = await _accountService.Register(registerDto);
        return result.Succeeded == true ? Ok(result) : BadRequest(result);
    }
}