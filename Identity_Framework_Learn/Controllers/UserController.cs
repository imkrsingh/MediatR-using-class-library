using System.Data;
using AutoMapper;
using data.viewmodel.models;
using Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Commands.User;
using Service.Query.User;
using Service.RequestModel;
using Service.UserGroup;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly GetUserDatas _getUserDatas;
        private readonly PutUserDatas _putUserData;

        public UserController(UserService userService,  PutUserDatas putUserData ,GetUserDatas getUserDatas, RoleManager<IdentityRole> roleManager, IMapper mapper , UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userService = userService;
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _mediator = mediator;
            _getUserDatas = getUserDatas;
            _putUserData = putUserData;
        }


        //[HttpGet]
        //[Route("GetRoles")]
        //public async Task<ActionResult<Roleresponse>> GetRoles()
        //{
        //    var roles = await _roleManager.Roles.ToListAsync();
        //    var roleResponses = roles.Select(r => new Roleresponse
        //    {
        //        Id = r.Id,
        //        RoleName = r.Name
        //    }).ToList();

        //    return Ok(roleResponses);
        //}


        //[HttpGet]
        //[Route("GetUserById/{userId}")]
        //public async Task<ActionResult<AppResponse<UserRegisterRequest>>> GetUserById(string userId)
        //{
        //    var response = await _getUserDatas.GetUserByIdAsync(userId);
        //    return Ok(response);
        //}

        [HttpGet]
        [Route("GetUserDataById/{userId}")]
        public async Task<ActionResult<AppResponse<UserRegisterRequest>>> GetUserDataById(string userId)
        {
            var user = await _mediator.Send(new GetbyIdQuery(userId));
            {
                if(user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound();
                }
            }
            //var query = new GetByIdQuery { Id = userId };
            //var response = await _mediator.Send(query);
           // return Ok(response);
        }


        [HttpPost]
        [Route("registration")]
        public async Task<ActionResult<AppResponse>> PostUserData([FromBody] UserRegisterRequest userData)
        {
            var response = await _mediator.Send(new RegisterUserCommand(userData));

            var app_response = new AppResponse();

            if (response == true)
            {
                return app_response.SetSuccessResponce("user", "User registered successfully");
            }
            else
            {
                return app_response.SetErrorResponce("error", "Something went wriong while processing user registration");
            }
        }


        [HttpPut]
        [Route("update/{userId}")]
        public async Task<ActionResult> PutUserData(string userId, [FromBody] UserRegisterRequest userData)
        {
            var response = await _mediator.Send(new UpdateUserCommand(userId, userData));

            if (response)
            {
                return Ok(new { message = "User updated successfully" });
            }
            else
            {
                return BadRequest(new { error = "Failed to update user" });
            }
        }


        [HttpDelete]
        [Route("deleteUserData/{id}")]
        public async Task<ActionResult<AppResponse>> DeleteUserData([FromRoute] string id)
        {
            var response = await _mediator.Send(new DeleteUserCommand(id));

            var app_response = new AppResponse();

            if (response == true)
            {
                return app_response.SetSuccessResponce("user", "User deleted successfully");
            }
            else
            {
                return app_response.SetErrorResponce("error", "Something went wrong while processing user deletion");
            }
        }






        [HttpGet]
        [Route("GetRoles")]
        public async Task<ActionResult<RoleResponse>> GetRoles()
        {

            var roles = await _roleManager.Roles.ToListAsync();
            var roleResponses = _mapper.Map<List<RoleResponse>>(roles);

            return Ok(roleResponses);
        }

       
        [HttpPost]
        [Route("UserLogin")]
        public async Task<ActionResult<AppResponse<bool>>> UserLogin(LoginRequest req)

        {
            var response = await _userService.UserLoginAsync(req);
            return Ok(response);
        }



        [HttpDelete]
        [Route("User/{id}")]
        public async Task<ActionResult<AppResponse<bool>>> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new AppResponse<bool>().SetSuccessResponce(true));
            }
            else
            {
                return StatusCode(500, new AppResponse<bool> { Message = "User deletion failed" });
            }
        }


        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(ApplicationRoleRequest roleModel)
        {
            if (ModelState.IsValid)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(roleModel.Name);
                if (roleExists)
                {
                    ModelState.AddModelError("", "Role Already Exists");
                }
                else
                {
                    var identityRole = new IdentityRole // Changed to IdentityRole
                    {
                        Name = roleModel?.Name,
                        ConcurrencyStamp = Guid.NewGuid().ToString() // Pass ConcurrencyStamp 
                    };
                    var result = await _roleManager.CreateAsync(identityRole); // Changed to identityRole
                    if (result.Succeeded)
                    {
                        var createdRole = new { Id = identityRole.Id, Name = identityRole.Name }; // Create anonymous object with only Id and Name
                        return Ok(createdRole); // Return only Id and Name if role creation is successful
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return BadRequest(ModelState); // Return BadRequest if ModelState is not valid
        }




        [HttpDelete]
        [Route("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
        }


    }
}