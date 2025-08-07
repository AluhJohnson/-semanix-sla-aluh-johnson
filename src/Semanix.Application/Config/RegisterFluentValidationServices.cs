using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Semanix.Application.Ticket.Validators;

namespace Semanix.Application.Config;

public static class RegisterFluentValidationServices
{
    public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssemblyContaining<CreateTicketDtoValidator>();

        return services;
    }
}