﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareClassWebAPI.Entities;
using ShareClassWebAPI.ViewModels;

namespace ShareClassWebAPI.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Account/SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel signUpViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = new User
                    {
                        Login = signUpViewModel.Login,
                        UserName = signUpViewModel.Login,
                        Name = signUpViewModel.Name
                    };

                    if (signUpViewModel.Login.Length > 15)
                    {
                        throw new Exception();
                    }

                    var createUserResult = await _userManager.CreateAsync(user, signUpViewModel.Password);

                    if (createUserResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Constants.userRole);
                        await _signInManager.SignInAsync(user, false);
                        return Ok(new { message = "Добавлен новый пользователь: " + user.UserName, userName = user.UserName, Constants.userRole, userId = user.Id });
                    }
                    else
                    {
                        foreach (var error in createUserResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                throw new Exception();
            }
            catch
            {
                var errorMsg = new
                {
                    message = "Недопустимые входные данные",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Conflict(errorMsg);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Account/SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInViewModel signInViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var passwordSignInResult = await _signInManager.PasswordSignInAsync(signInViewModel.Login, signInViewModel.Password, signInViewModel.RememberMe, false);

                    if (passwordSignInResult.Succeeded)
                    {
                        var user = await _userManager.FindByNameAsync(signInViewModel.Login);
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null && roles.Count > 0)
                        {
                            var userRole = roles[0];
                            return Ok(new { message = "Выполнен вход", userName = signInViewModel.Login, userRole, userId = user.Id });
                        }
                        return Ok(new { message = "Выполнен вход", userName = signInViewModel.Login, userId = user.Id });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный логин/пароль");
                    }
                }

                throw new Exception();
            }
            catch
            {
                var errorMsg = new
                {
                    message = "Вход не выполнен",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Conflict(errorMsg);
            }
        }

        [HttpPost]
        [Route("api/Account/SignOut")]
        public new async Task<IActionResult> SignOut()
        {
            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Unauthorized(new { message = "Вход не выполнен" });
            }

            await _signInManager.SignOutAsync();
            return Ok(new { message = "Выполнен выход", userName = curentUser.UserName });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Account/IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            User curentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (curentUser == null)
            {
                return Unauthorized(new { message = "Вход не выполнен" });
            }

            var roles = await _userManager.GetRolesAsync(curentUser);
            if (roles != null && roles.Count > 0)
            {
                var userRole = roles[0];
                return Ok(new { message = "Сессия активна", userName = curentUser.UserName, userRole, userId = curentUser.Id });
            }
            return Ok(new { message = "Сессия активна", userName = curentUser.UserName });
        }
    }
}
