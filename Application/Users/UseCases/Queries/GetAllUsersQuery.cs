using Application.Interfaces;
using Application.Users.UseCases.Dtos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {

    }

    
    
}
