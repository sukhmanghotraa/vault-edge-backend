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
                .Map(dest => dest.AccessToken, src => src.AccessToken)
                .Map(dest => dest, src => src.Customer);
        }
    }
}
