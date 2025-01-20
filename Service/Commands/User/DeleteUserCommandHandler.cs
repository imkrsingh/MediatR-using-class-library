

//using Service.Commands.Base;

//namespace Service.Commands.User
//{
//    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
//    {
//        public Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}


using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Service.Commands.User;
using Service.UserGroup;

namespace Service.UserGroup
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly DeleteUserDatas _deleteUserDatas;

        public DeleteUserCommandHandler(DeleteUserDatas deleteUserDatas)
        {
            _deleteUserDatas = deleteUserDatas;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _deleteUserDatas.DeleteUserByIdAsync(request?.id);
        }
    }
}

