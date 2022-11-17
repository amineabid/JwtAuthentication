using JwtAuthentication.Server.Models;
using JwtAuthentication.Shared;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JwtAuthentication.Server.Services
{
    public interface IAccountServices
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserFindByIdAsync(string id);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUserNameAsync(string id);

        Task<IdentityResult> CreateUserAsync(SignUpDto userModel);
        Task<User> DeleteUserAsync(string id);

        Task<SignInResult> PasswordSignInAsync(SignInDto signInModel);

        Task SignOutAsync();

        Task<IdentityResult> ChangePasswordAutreUserAsync(string userId, string Newpassword);

        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto model);
        Task<IdentityResult> UpdateAutreUser(string Id, SignUpDto Newuser);


        Task<List<IdentityRole>> GetRoles();
        Task<IdentityResult> CreateRoles(IdentityRole role);


        Task<List<User>> GetAllUsers();
        Task<List<IdentityRole>> GetRolesUsers();
        Task<List<IdentityUserRole<string>>> GetRoleUsers();
        Task<IdentityResult> SetRoleUsers(string userid, List<string> roles);
        Task<List<User>> GetAllClient();


        Task<string> GenerateJSONWebTocken(User identityUser);
    }
}