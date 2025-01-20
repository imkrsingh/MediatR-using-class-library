﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Service.Query.Base
{
    public interface IQuery<out TResponse> : IRequest<TResponse>    
    {
    }
}
