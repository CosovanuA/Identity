using FluentValidation;
using FluentValidation.Validators;
using Identity.API.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Domain.Validators
{
    public class AddUserDTOValidator : AbstractValidator<AddUserDTO>
    {
        public AddUserDTOValidator()
        {
            RuleFor(userDTO => userDTO.Username)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(50);

            RuleFor(userDTO => userDTO.Password)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(50);

            RuleFor(userDTO => userDTO.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible);
        }
    }
}
