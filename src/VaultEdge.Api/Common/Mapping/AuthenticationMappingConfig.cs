using Mapster;
using VaultEdge.Api.Authentication;
using VaultEdge.Application.Authentication.Commands.Signup;
using VaultEdge.Application.Authentication.Common;
using VaultEdge.Application.Authentication.Queries.Signin;

namespace VaultEdge.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SignupRequest, SignupCommand>();

            config.NewConfig<SigninRequest, SigninQuery>();

            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(dest => dest.Token, src => src.Token)
                .Map(dest => dest, src => src.User);
        }
    }
}
