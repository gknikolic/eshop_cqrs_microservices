namespace Shopping.Web.Models.Account;

public record RegisterRequestDto(string username, string email, string password, string firstName, string lastName);
