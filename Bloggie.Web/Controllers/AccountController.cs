﻿using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public AccountController(UserManager<IdentityUser> userManager, 
			SignInManager<IdentityUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

        [HttpGet]
		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			var identityUser = new IdentityUser
			{
				UserName = registerViewModel.Username,
				Email = registerViewModel.Email,
			};

			var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
			if (identityResult.Succeeded)
			{
				// Assign this user the "User" role
				var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
				if (roleIdentityResult.Succeeded)
				{
					//Show success notification
					return RedirectToAction("Register");
				}
			}

			//Show error notification
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
				loginViewModel.Password, false, false);
			if (signInResult != null && signInResult.Succeeded)
			{
				return RedirectToAction("Index","Home");
			}
			//Show error notification
			return View();
		}
    }
}
