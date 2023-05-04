using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace EDennis.MvcUtils
{
    /// <summary>
    /// Constants for Authentication Schemes
    /// </summary>
    public class AuthenticationSchemeConstants
    {
        /// <summary>
        /// Scheme that builds an identity with a fake user claim
        /// </summary>
        public const string FakeAuthenticationScheme = "Fake";

        public const string MsalAndFake = OpenIdConnectDefaults.AuthenticationScheme + ","
            + FakeAuthenticationScheme;

    }
}
