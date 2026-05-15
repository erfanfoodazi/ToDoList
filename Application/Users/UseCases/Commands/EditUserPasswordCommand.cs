using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Commands
{
    public class EditUserPasswordCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; } 
        public string? ConfirmPassword { get; set; }
    }
}