using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Service.Query.Base;
using Service.RequestModel;
using Service.UserGroup;

namespace Service.Query.User
{
    public class GetByHandler : IQueryHandler<GetbyIdQuery, UserRegisterRequest>
    {
        private GetUserDatas _getUserDatas;

        public GetByHandler(GetUserDatas getUserDatas)
        {
            _getUserDatas = getUserDatas;
        }

        public async Task<UserRegisterRequest> Handle(GetbyIdQuery query, CancellationToken cancellationToken)
        {
            return await _getUserDatas.GetUserByIdAsync(query.Id);
        }
    }
}
