using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Service.Commands.Base;
using Service.Commands.User;
using Service.RequestModel; 
using Service.UserGroup;


//public class UpdateUserCommand : IRequest<bool>
//{
//    public string Id { get; set; }
//    public UserRegisterRequest UserUpdateRequest { get; set; }
//}

//public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
//{
//    private readonly PutUserDatas _putUserDatas;

//    public UpdateUserCommandHandler(PutUserDatas putUserDatas)
//    {
//        _putUserDatas = putUserDatas ?? throw new ArgumentNullException(nameof(putUserDatas));
//    }

//    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
//    {
//        if (request == null)
//        {
//            throw new ArgumentNullException(nameof(request));
//        }

//        return await _putUserDatas.UpdateUserAsync(request.Id, request.UserUpdateRequest);
//    }
//}


using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Service.UserGroup;

namespace Service.Commands.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly PutUserDatas _putUserDatas;

        public UpdateUserCommandHandler(PutUserDatas putUserDatas)
        {
            _putUserDatas = putUserDatas ?? throw new ArgumentNullException(nameof(putUserDatas));
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _putUserDatas.UpdateUserAsync(request.Id, request.UserUpdateRequest);
        }
    }
}




