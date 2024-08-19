namespace Shopping.Web.Models.Account;

public record LoginResponseDto(bool isLogedIn, string? token, string? refreshToken);