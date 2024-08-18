namespace Shopping.Web.Dtos.Account;

public record RegisterRequestDto(string username, string email, string password, string firstName, string lastName);
