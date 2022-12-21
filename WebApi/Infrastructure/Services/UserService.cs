using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApi.Infrastructure.Data.Dtos;

namespace WebApi.Infrastructure.Services;

public class UserService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(DataContext context,IConfiguration configuration,UserManager<IdentityUser> userManager)
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
    }
    
    public async Task<IdentityResult> Register(RegisterDto registerDto)
    {
        var user = new IdentityUser()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };
        var result = await _userManager.CreateAsync(user,registerDto.Password);
        return result;
    }
    
    public async Task<TokenDto> Login(LoginDto login)
    {
        var user = await _userManager.FindByNameAsync(login.Username);
        if (user != null)
        {
            var validatePassword = new PasswordValidator<IdentityUser>();
            var result = await  validatePassword.ValidateAsync(_userManager, user, login.Password);
            if (result.Succeeded == false)
            {
                return null;
            }

            return await GenerateJwtToken(user);

        }

        return null;
    }


    //Method to generate The Token
    private async Task<TokenDto> GenerateJwtToken(IdentityUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, "Admin"),
        };
        //add roles
        
        // var roles = await _userManager.GetRolesAsync(user);
        // claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));
        //
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: credentials
        );

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = securityTokenHandler.WriteToken(token);
        return new TokenDto()
        {
            Token = tokenString
        };
    }
}