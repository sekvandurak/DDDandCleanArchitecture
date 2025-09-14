using BuberDinner.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

// IRequest<TResponse> comes from MediatR
public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;