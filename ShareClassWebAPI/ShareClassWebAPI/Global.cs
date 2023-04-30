using Microsoft.AspNetCore.Identity;
using ShareClassWebAPI.Entities;

namespace ShareClassWebAPI
{
    public static class Global
    {
        public static readonly UserManager<User> _userManager;
        public static readonly SignInManager<User> _signInManager;


    }
}
