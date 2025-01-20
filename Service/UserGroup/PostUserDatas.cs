using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Identity;
using Service.RequestModel;

namespace Service.UserGroup
{
    public class PostUserDatas
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PostUserDatas(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateUserAsync(UserRegisterRequest userRequest)
        {
            var newUser = new ApplicationUser
            {
                UserName = userRequest.UserName,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNo
            };

            var result = await _userManager.CreateAsync(newUser, userRequest.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(newUser, userRequest.Role);

                if (roleResult.Succeeded)
                {
                    return true; 
                }
                else
                {
                    await _userManager.DeleteAsync(newUser);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
