using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Identity;
using Service.RequestModel;

namespace Service.UserGroup
{
    public class DeleteUserDatas
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserDatas(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserRegisterRequest> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            var userRegisterRequest = new UserRegisterRequest
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNo = user.PhoneNumber,
                Role = roles.FirstOrDefault()
            };

            return userRegisterRequest;
        }

        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false; // User not found
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
