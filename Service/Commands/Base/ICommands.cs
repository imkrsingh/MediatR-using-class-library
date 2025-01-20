using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Service.Commands.Base
{
    //public interface ICommand<out BaseResponse> : IRequest<BaseResponse>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
