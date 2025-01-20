//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Service.Commands.Base;
//using Service.RequestModel;

//namespace Service.Commands.User
//{
//    public record UpdateUserCommand(string id, RequestModel.UserRegisterRequest userData) : ICommand<bool>
//    {
//        public UserRegisterRequest UserUpdateRequest { get; internal set; }
//    }
//}


using MediatR;
using Service.Commands.Base;
using Service.RequestModel;

namespace Service.Commands.User
{
    public class UpdateUserCommand : ICommand<bool>
    {
        public string Id { get; }
        public UserRegisterRequest UserUpdateRequest { get; }

        public UpdateUserCommand(string id, UserRegisterRequest userUpdateRequest)
        {
            Id = id;
            UserUpdateRequest = userUpdateRequest;
        }
    }
}

