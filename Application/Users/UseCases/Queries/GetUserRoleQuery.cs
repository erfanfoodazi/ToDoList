using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Queries
{
    public class GetUserRoleQuery : IRequest<IList<string>>
    {
        public int Id { get; set; }
    }
}
