
using MediatR;
using Service.Query.Base;
using Service.RequestModel;

namespace Service.Query.User
{
    public record GetbyIdQuery(string Id) : IQuery<UserRegisterRequest>;
}