using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EDennis.MvcUtils
{
    public class MvcAuthenticationStateProvider
    {
        public ClaimsPrincipal User { get; set; }

        public virtual Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(new AuthenticationState(User));
    }
}
