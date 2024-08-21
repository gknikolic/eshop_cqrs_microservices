
using BuildingBlocks.Authorization;
using Customer.API.Database.Entities;
using Customer.API.Dtos;

namespace Customer.API.Identity.GetAllUsers;

public record GetUsersQuery : IQuery<GetUsersResult>;
public record GetUsersResult(IEnumerable<UserDto> Users);

public class GetUsersHandler(UserManager<User> _userManager)
    : IQueryHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userManager.Users.ToList();

        var userList = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = (RoleEnum) Enum.Parse(typeof(RoleEnum), roles.FirstOrDefault() ?? RoleEnum.User.ToString()),
                FullName = user.FullName,
                EmailConfirmed = user.EmailConfirmed
            };

            userList.Add(userDto);
        }

        return new GetUsersResult(userList);
    }
}
