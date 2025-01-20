using System.Data;
using Data;
using Microsoft.AspNetCore.Identity;
using Service.RequestModel;

namespace Service.UserGroup
{
    //public class UserRegisterRequest
    //{
    //    public string UserName { get; set; } = "";
    //    public string Email { get; set; } = "";
    //    public string Password { get; set; } = "";
    //    public string PhoneNo { get; set; }
    //    public string Address {  get; set; } 
    //}
    public partial class UserService
    {
        public async Task<AppResponse<bool>> UserRegisterAsync(UserRegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AppResponse<bool>().SetErrorResponce("email", "Email already exists");
            }

            var user = new ApplicationUser()
            {
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = request.Email,
                PhoneNumber = request.PhoneNo
            };

            //var result = await _userManager.CreateAsync(user, request.Password);
            //if (!result.Succeeded)
            //{
            //    var errorDictionary = GetRegisterErrors(result);
            //    return new AppResponse<bool>().SetErrorResponce(errorDictionary);
            //}



            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                return new AppResponse<bool>().SetErrorResponce("role", "Role does not exist");
            }

            var role = await _roleManager.FindByNameAsync(request.Role);
            if (role == null)
            {
                return new AppResponse<bool>().SetErrorResponce("role", "Role does not exist");
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errorDictionary = GetRegisterErrors(result);
                return new AppResponse<bool>().SetErrorResponce(errorDictionary);
            }
            await _userManager.AddToRoleAsync(user, role.Name);
            return new AppResponse<bool>().SetSuccessResponce(true);
        }



        private Dictionary<string, string[]> GetRegisterErrors(IdentityResult result)
        {
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = new string[] { error.Description };
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return errorDictionary;
        }
    }
}
