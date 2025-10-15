using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Application.Features.Groups.Command.Update
{
    public class UpdateGroupCommandValidator:AbstractValidator<UpdateGroupCommand>
    {
        public UpdateGroupCommandValidator()
        {
            
        }
    }
}
