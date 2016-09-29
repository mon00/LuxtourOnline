﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.Security;

namespace LuxtourOnline.Models
{

    #region System 

    public class AppUser : IdentityUser
    {
        public AppUser() : base() { }
        public AppUser(string email) : base(email)
        {
            Email = email;
        }

        public AppUser(string email, string fullName, string ip, string phone) : base(email)
        {
            Email = email;
            FullName = fullName;
            PhoneNumber = phone;
            RegIp = ip;
            RegDate = DateTime.Now.Date.ToShortDateString();
        }

        public string FullName { get; set; }
        public string RegIp { get; set; }
        public bool Active { get; set; } = true;
        public string RegDate { get; set; } = DateTime.Now.Date.ToShortDateString();
    }

    public class AppUserRole : IdentityRole
    {
        public AppUserRole()
        {
        }

        public AppUserRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }

        public string Description { get; set; }
    }

    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
            PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 7,
                RequireLowercase = false,
                RequireDigit = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };
        }

        public async void AddUserAsync(string fullName, string email, string password, string ip)
        {
            AppUser user = new AppUser() { Email = email, UserName = fullName, PasswordHash = (new PasswordHasher().HashPassword(password)), RegIp = ip, RegDate = DateTime.Now.Date.ToShortDateString() };
            await Store.CreateAsync(user);
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<SiteDbContext>()));

            // optionally configure your manager
            // ...

            return manager;
        }

        public static AppUserManager Create(IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<SiteDbContext>()));
            return manager;
        }

    }

    public class AppSignInManager : SignInManager<AppUser, string>
    {
        public AppSignInManager(UserManager<AppUser, string> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }

    #endregion System

    public class SelfUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public SelfUserModel()
        {

        }

        public SelfUserModel(AppUser user, string roles)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            Role = roles;
        }
    }

    public class ChangePassword
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirm { get; set; }
    }

    public class ChangeEmail
    {


        [Required]
        public string Id { get; set; }

        [EmailAddress]
        [Required]
        public string NewEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string  Password { get; set; }

        public ChangeEmail()
        {

        }

        public ChangeEmail(AppUser user)
        {
            Id = user.Id;
        }
    }

    public class ChangeFullName
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ChangeFullName()
        {

        }

        public ChangeFullName(AppUser user)
        {
            Id = user.Id;
            FullName = user.FullName;
        }

    }


    #region Manager

    public class UpdateUserModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        
        public string Role { get; set; }

        public string NewRole { get; set; }

        public UpdateUserModel()
        {

        }

        public UpdateUserModel(AppUser user, string role)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Role = role;
        }

    }

    public class ListUserModel
    {
        public PagingInfo Paging { get; set; }
        public List<DisplayUserModel> Users { get; set; }
    }

    public class DisplayUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Roles { get; set; }
        public string CreatedDate { get; set; }

    }

    public class CreateUserModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RoleToUse { get; set; }
    }

    public class RemoveUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }


        public RemoveUserModel()
        {

        }

        public RemoveUserModel(AppUser user)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
        }
    }

    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public bool Remember { get; set; }

        public string RedirectUrl { get; set; }

    }

    public class UserRegistrationModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string Confirm { get; set; }
    }

    #endregion Manager

}