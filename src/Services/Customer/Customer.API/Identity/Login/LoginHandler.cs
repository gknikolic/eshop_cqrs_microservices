using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Customer.API.Identity.Login;

public record LoginCommand(string email, string password) : ICommand<LoginResult>;
public record LoginResult(bool success, string token = default!);

public class LoginCommadValidator : AbstractValidator<LoginCommand>
{
    public LoginCommadValidator()
    {
        RuleFor(x => x.email).NotEmpty().EmailAddress();
        RuleFor(x => x.password).NotEmpty();
    }
}

public class LoginHandler(UserManager<User> _userManager, IConfiguration _configuration)
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.email);
        if (user != null && await _userManager.CheckPasswordAsync(user, request.password))
        {
            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);
            return new LoginResult(true, token);
        }
        else
        {
            return new LoginResult(false);
        }
    }

    // Helper Method to Generate JWT Token
    private string GenerateJwtToken(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Name, (user.FirstName + " " + user.LastName) ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }.Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var keyString = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(keyString) || keyString.Length < 16)
        {
            throw new ArgumentException("The JWT key must be at least 16 characters long.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
