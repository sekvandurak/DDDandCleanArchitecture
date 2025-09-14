using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwTokenGenerator _jwTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IUserRepository userRepository, IJwTokenGenerator jwTokenGenerator)
    {
        _userRepository = userRepository; 
        _jwTokenGenerator = jwTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        //1. Validate the user exists
        if (_userRepository.GetUserByEmail(query.Email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }
        //2. Validate the password is correct

        if (user.Password != query.Password)
        {
            return new[] { Errors.Authentication.InvalidCredentials };
        }

        //3. Create JWT token
        var token = _jwTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
                user,
                token);
    }
}