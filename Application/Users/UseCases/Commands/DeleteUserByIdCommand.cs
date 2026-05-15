using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Commands
{
    public class DeleteUserByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
