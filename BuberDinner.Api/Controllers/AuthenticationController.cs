using System.Threading.Tasks;
using BuberDinner.Application.Authentication;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AutheticationController : ApiController
{
    ISender _mediator;
    IMapper _mapper; // for Mapster


    public AutheticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request); // using Mapster mapping

        //manual mapping
        /*  var command = new RegisterCommand(   
               request.FirstName,
               request.LastName,
               request.Email,
               request.Password); */

        ErrorOr<AuthenticationResult> authenticationResult = await _mediator.Send(command);

        return authenticationResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), // using Mapster mapping
            /* authResult => Ok(MapAuthresult(authResult)),  manual mapping */
            errors => Problem(errors)
        );

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request); // using Mapster mapping
        /*  var query = new LoginQuery(
             request.Email,
             request.Password); */

        ErrorOr<AuthenticationResult> authenticationResult = await _mediator.Send(query); // sending to the handlee

        if (authenticationResult.IsError && authenticationResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: authenticationResult.FirstError.Description);
        }
        return authenticationResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), // using Mapster mapping
            /* authResult => Ok(MapAuthresult(authResult)),  manual mapping */
            errors => Problem(errors)
        );

    }

    //manual mapping
    /*     private static AuthenticationResponse MapAuthresult(AuthenticationResult authenticationResult)
        {
            return new AuthenticationResponse(
                authenticationResult.User.Id,
                authenticationResult.User.FirstName,
                authenticationResult.User.LastName,
                authenticationResult.User.Email,
                authenticationResult.Token);
        }
     */
}
