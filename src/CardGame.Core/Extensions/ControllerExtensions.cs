using System.Reflection;
using CardGame.Core.Services.Implementations;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CardGame.Core.Extensions;

public static class ControllerExtensions
{
    public static void AddControllerExtension(this IServiceCollection services)
    {
        services
            .AddControllers(options => options.Filters.Add(typeof(ValidateModelStateAttribute)))
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });
    }
}