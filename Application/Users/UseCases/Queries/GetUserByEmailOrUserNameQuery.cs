using Application.Users.UseCases.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Queries
{
    public class GetUserByEmailOrUserNameQuery : IRequest<UserDto>
    {
        public string? EmailOrUserName { get; set; }
    }
}
