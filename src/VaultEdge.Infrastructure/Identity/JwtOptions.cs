using System;
using System.Collections.Generic;
using System.Text;

namespace VaultEdge.Infrastructure.Identity
{
    public class JwtOptions
    {
        public string Issuer { get; init; }

        public string Audience { get; init; }

        public string SecretKey { get; init; }
    }
}
