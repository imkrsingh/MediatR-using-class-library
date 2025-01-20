using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Identity;
using Service.RequestModel;

namespace Service.UserGroup
{
    public class GetUserDatas
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserDatas(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserRegisterRequest> GetUserByIdAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
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
    }
}
