﻿
namespace Customer.API.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; internal set; }
}
