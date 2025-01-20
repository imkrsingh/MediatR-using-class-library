using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Service.RequestModel;
using Data;

namespace Service.UserGroup
{
    public class PutUserDatas
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PutUserDatas(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> UpdateUserAsync(string id, UserRegisterRequest userUpdateRequest)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false; // User not found
            }

            // Update user properties
            user.UserName = userUpdateRequest.UserName;
            user.Email = userUpdateRequest.Email;
            user.PhoneNumber = userUpdateRequest.PhoneNo;

            // Update user roles if necessary
            if (!string.IsNullOrEmpty(userUpdateRequest.Role))
            {
                // Remove all existing roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                // Add new role
                await _userManager.AddToRoleAsync(user, userUpdateRequest.Role);
            }

            // Update user in the database
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
