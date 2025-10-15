using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Features.Groups.Command.Create
{
    public class CreateGroupCommandValidator:AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupCommandValidator()
        {
            
        }
    }
}
