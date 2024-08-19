using Customer.API.Database.Entities;
using Customer.API.Identity.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Customer.API.Services;

/// <summary>
/// Represents an authentication service for registering and logging in users.
/// </summary>
public class AuthService(UserManager<User> _userManager, IConfiguration _configuration)
    : IAuthService
{
    /// <summary>
    /// Registers a new user with the provided user details.
    /// </summary>
    /// <param name="model">The user details for registration.</param>
    /// <returns>A <see cref="RegisterUserResult"/> indicating the registration result.</returns>
    public async Task<RegisterUserResult> Register(RegisterUserModel model)
    {
        var result = new RegisterUserResult();

        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var createUserResult = await _userManager.CreateAsync(user, model.Password);
        if (!createUserResult.Succeeded)
        {
            result.IsRegistered = false;
            result.Message = createUserResult.Errors.FirstOrDefault()?.Description;
            return result;
        }

        // Optional: Assign "User" role by default to the new user
        await _userManager.AddToRoleAsync(user, "User");

        result.IsRegistered = true;
        result.User = user;

        return result;
    }

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="credentials">The login credentials.</param>
    /// <returns>A <see cref="LoginResult"/> indicating the login result.</returns>
    public async Task<LoginResult> Login(LoginUserModel credentials)
    {
        var user = await _userManager.FindByEmailAsync(credentials.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, credentials.Password))
        {
            var tokens = await GenerateTokens(user);

            return new LoginResult(true, tokens.jwtToken, tokens.refreshToken);
        }
        else
        {
            return new LoginResult(false);
        }
    }

    /// <summary>
    /// Impersonates a user with the provided user ID.
    /// </summary>
    /// <param name="userId">The ID of the user to impersonate.</param>
    /// <returns>A <see cref="LoginResult"/> indicating the impersonation result.</returns>
    public async Task<LoginResult> ImpersonateUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var tokens = await GenerateTokens(user);

            return new LoginResult(true, tokens.jwtToken, tokens.refreshToken);
        }
        else
        {
            return new LoginResult(false);
        }
    }

    /// <summary>
    /// Refreshes the access token using the provided token and refresh token.
    /// </summary>
    /// <param name="token">The access token.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <returns>A <see cref="LoginResult"/> indicating the token refresh result.</returns>
    public async Task<LoginResult> RefreshToken(string token, string refreshToken)
    {
        // Check if the token is valid

        var principal = GetTokenPrincipal(token);

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId is null)
            return new LoginResult(false);

        var identityUser = await _userManager.FindByIdAsync(userId);

        // Check if the user exists and the refresh token is valid
        if (identityUser is null || identityUser.RefreshToken != refreshToken || identityUser.RefreshTokenExpiry < DateTime.Now)
            return new LoginResult(false);

        // Generate new tokens
        var tokens = await GenerateTokens(identityUser);

        // Return the new tokens
        return new LoginResult(true, tokens.jwtToken, tokens.refreshToken);
    }

    private ClaimsPrincipal? GetTokenPrincipal(string token)
    {

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Key").Value));

        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = securityKey,
            ValidateLifetime = false,
            ValidateActor = false,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    /// <summary>
    /// Generates JWT token and refresh token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the tokens.</param>
    /// <returns>A tuple containing the JWT token and refresh token.</returns>
    private async Task<(string jwtToken, string refreshToken)> GenerateTokens(User user)
    {
        var jwtToken = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        // store refresh token in user table 
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:RefreshExpiration"]!));
        await _userManager.UpdateAsync(user);

        return (jwtToken, refreshToken);
    }

    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>The generated JWT token.</returns>
    private async Task<string> GenerateJwtToken(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, (user.FirstName + " " + user.LastName) ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var keyString = _configuration["JwtSettings:Key"];
        if (string.IsNullOrEmpty(keyString) || keyString.Length < 16)
        {
            throw new ArgumentException("The JWT key must be at least 16 characters long.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:AccessExpiration"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <returns>The generated refresh token.</returns>
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
