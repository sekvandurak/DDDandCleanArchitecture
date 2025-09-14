using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register;

//  commandHandler comes from MediatR library
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwTokenGenerator _jwTokenGenerator;
    private readonly IUserRepository _userRepository;
    public RegisterCommandHandler(IJwTokenGenerator jwTokenGenerator, IUserRepository userRepository)
    {
        _jwTokenGenerator = jwTokenGenerator;
        _userRepository = userRepository;
    }
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // 1. Check if user already does not exists
        var existingUser = _userRepository.GetUserByEmail(command.Email);
        if (existingUser is not null)
        {
            //throw new Exception("User with given email already exists");
            return Errors.User.DuplicateEmail;
        }

        //2. Create user (generate unique ID) and persist to DB

        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password
        };
        _userRepository.Add(user);

        //3. Create JWT token

        var token = _jwTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}