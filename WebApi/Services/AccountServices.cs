using JwtAuthentication.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JwtAuthentication.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using JwtAuthentication.Shared;

namespace JwtAuthentication.Server.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;
        private IPasswordHasher<User> _passwordHasher;
        private readonly ApplicationDbContext _context;

        public AccountServices(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<User> passwordHasher,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _context = context;
        }
        public async Task<List<User>> GetAllClient()
        {
            // ignore role Intermidaire and this is id of role Intermidaire
            var listIdsUsersRoles = await _context.UserRoles.Select(a => a.UserId).ToListAsync();
            var listUsers = await _userManager.Users.ToListAsync();
            var listClients =  listUsers.Where(u => listIdsUsersRoles.IndexOf(u.Id) == -1).ToList();

            return listClients;
        }

        private bool ClientExists(string id,List<string> idsUsers)
        {
            
            return idsUsers.Any(e => e == id);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<User> GetUserFindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> CreateUserAsync(SignUpDto userModel)
        {
            var user = new User()
            {
                UserName = userModel.Email,
                CustomerName = userModel.Nom,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                DateOfBirth = userModel.DateOfBirth,
                City = userModel.City

            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(SignInDto signInModel)
        {
            var resultSingIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, true); ;
            return resultSingIn;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<IdentityResult> ChangePasswordAutreUserAsync(string userId,string Newpassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, Newpassword);
        }



        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }
        public async Task<IdentityResult> UpdateAutreUser(string Id,SignUpDto Newuser)
        {
            User user = await _context.Users.Where(a=>a.Id == Id).FirstAsync() ;
            user.CustomerName = Newuser.Nom;
            user.DateOfBirth = Newuser.DateOfBirth;
            user.Email = Newuser.Email;
            user.City = Newuser.City;
            user.PhoneNumber = Newuser.PhoneNumber;
            user.UserName = Newuser.Email;
            
            IdentityResult result = await _userManager.UpdateAsync(user);
            return result;
        }
        public async Task<List<IdentityRole>> GetRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        public async Task<IdentityResult> CreateRoles(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
        public async Task<List<IdentityRole>> GetRolesUsers()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }
        public async Task<List<IdentityUserRole<string>>> GetRoleUsers()
        {
            var roles = await _context.UserRoles.ToListAsync();
            return roles;
        }
        public async Task<IdentityResult> SetRoleUsers(string userid,List<string> roles)
        {
            try
            {
                User user = await _context.Users.FindAsync(userid); //Where(u => userid.Equals(u.Id, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var listroles = await _context.Roles.Where(r=> roles.Contains(r.Id)).ToListAsync();
                var troles = await _context.Roles.Select(r => r.NormalizedName).ToListAsync();
                //var rolesremoved =await _userManager.RemoveFromRolesAsync(user, troles);
                foreach (var roler in troles)
                {
                    await _userManager.RemoveFromRoleAsync(user, roler);
                }
                var rolesadded = await _userManager.AddToRolesAsync(user, listroles.Select(r => r.NormalizedName.ToUpper()).ToList());
                return rolesadded;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<int> ChangeRoleUsers(IdentityUserRole<string> model)
        {
            var findIdentityRole = await _context.UserRoles.FindAsync(model.UserId);
            findIdentityRole.RoleId = model.RoleId;
            _context.UserRoles.Update(findIdentityRole);
            _context.Entry(findIdentityRole).State = EntityState.Modified;
            var roles = await _context.SaveChangesAsync();
            return roles;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> DeleteUserAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return user;
                }
                    
            }
            return null;
        }

        public async Task<User> GetUserByUserNameAsync(string UserName)
        {
            var VerifUser = await _context.Users.Where(a => a.UserName == UserName).ToListAsync();
            if (VerifUser!=null)
            {
                if (VerifUser.Count()>0)
                {
                    return VerifUser.FirstOrDefault();
                }
            }
            return null;
        }

        public async  Task<string> GenerateJSONWebTocken(User identityUser)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,identityUser.Email),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
            };
            IList<string> roleNames = await _userManager.GetRolesAsync(identityUser);
            claims.AddRange(roleNames.Select(a=> new Claim(ClaimsIdentity.DefaultRoleClaimType, a)));
            JwtSecurityToken token = new JwtSecurityToken(
                   _configuration["Jwt:Issuer"],
                   _configuration["Jwt:Issuer"],
                   claims,
                   null,
                   expires:DateTime.UtcNow.AddDays(28),
                   signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
