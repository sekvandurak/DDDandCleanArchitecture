using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using Mapster;

namespace BuberDinner.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    // IRegister is from Mapster
    public void Register(TypeAdapterConfig config)
    {

        // AuthenticationResult to AuthenticationResponse. 
        // src is AuthenticationResult, dest is AuthenticationResponse
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
        .Map(dest => dest.Token, src => src.Token)
        .Map(dest => dest, src => src.User);

    }
}