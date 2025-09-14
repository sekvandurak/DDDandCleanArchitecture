using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Common.Mapping;
using BuberDinner.Api.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BuberDinner.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ErrorHandlingFilterAttribute>();
        }); 
        services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
        services.AddMappings(); // extension method from Mapping/DependencyInjection.cs

        return services;
    }
}