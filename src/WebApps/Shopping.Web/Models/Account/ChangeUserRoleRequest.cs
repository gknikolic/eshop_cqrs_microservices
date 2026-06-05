namespace Shopping.Web.Models.Account;

public record ChangeUserRoleRequest(string userId, IList<string> newRoles);
