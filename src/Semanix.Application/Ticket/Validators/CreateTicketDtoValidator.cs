using FluentValidation;
using Semanix.Application.Ticket.Dto;

namespace Semanix.Application.Ticket.Validators;

public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
{
    public CreateTicketDtoValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("Title is required");//.MinimumLength(3)
           // .MaximumLength(50).WithMessage("Username must be between 3 and 50 characters")
            //.WithMessage("Username must not exceed 50 characters");
        //RuleFor(x => x.).NotEmpty().NotNull().WithMessage("Role is required");
    }
}