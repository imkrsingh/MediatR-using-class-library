
using Service.Commands.Base;
using Service.UserGroup;

namespace Service.Commands.User
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, bool>
    {
        private PostUserDatas _userService;

        public RegisterUserCommandHandler(PostUserDatas userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.CreateUserAsync(request?.request);

            return result;
        }
    }

}
