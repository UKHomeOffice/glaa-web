﻿using System;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IMapper mapper;
        private readonly UserManager<GLAAUser> userManager;

        public UserProfileController(IMapper mp, UserManager<GLAAUser> um)
        {
            mapper = mp;
            userManager = um;
        }

        // GET: /<controller>/
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var login = User.Identity.Name;
            var user = userManager.FindByEmailAsync(login).GetAwaiter().GetResult();
            var model = mapper.Map<AdminUserViewModel>(user) as UserViewModel;
            return View(model);
        }

        [HttpGet]
        public IActionResult EditName()
        {
            var login = User.Identity.Name;
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult EditEmail()
        {
            var login = User.Identity.Name;
            throw new NotImplementedException();
        }
    }
}
