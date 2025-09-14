using BuberDinner.Application;
using BuberDinner.Infrastructure;
using BuberDinner.Api;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddPresentation()
                    .AddApplication()
                    .AddInfrastructure(builder.Configuration);

}
var app = builder.Build();
{
    //app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}

