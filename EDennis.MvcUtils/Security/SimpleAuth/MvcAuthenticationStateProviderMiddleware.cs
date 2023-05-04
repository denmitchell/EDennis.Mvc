using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EDennis.MvcUtils
{
    internal class MvcAuthenticationStateProviderMiddleware<TAppUserRolesDbContext>
        where TAppUserRolesDbContext : AppUserRolesContextBase
    {
        private readonly RequestDelegate _next;
        private readonly SecurityOptions _securityOptions;
        private readonly RolesCache _rolesCache;

        public MvcAuthenticationStateProviderMiddleware(RequestDelegate next,
            IOptionsMonitor<SecurityOptions> securityOptions,
            RolesCache rolesCache)
        {
            _next = next;
            _securityOptions = securityOptions.CurrentValue;
            _rolesCache = rolesCache;
        }

        public async Task InvokeAsync(HttpContext context, MvcAuthenticationStateProvider authStateProvider,
                        TAppUserRolesDbContext appUserRolesDbContext)
        {
            if (context.User != null)
            {


                var userName = context.User.Claims.FirstOrDefault(c =>
                               c.Type.Equals(_securityOptions.IdpUserNameClaim,
                               StringComparison.OrdinalIgnoreCase))?.Value;

                if (userName != null)
                {
                    var role = await GetRoleAsync(userName,appUserRolesDbContext);


                    (context.User.Identity as ClaimsIdentity).AddClaims(
                        new Claim[]
                            {
                                new Claim("role", role),
                                new Claim(ClaimTypes.Role, role)
                            }
                        );

                    var identity = new ClaimsIdentity(
                        context.User.Claims
                            .Select(c => new Claim(c.Type, c.Value)));

                    authStateProvider.User = new ClaimsPrincipal(identity);
                }
            }

            await _next(context);
        }


        private async Task<string> GetRoleAsync(string userName, TAppUserRolesDbContext appUserRolesDbContext)
        {

            if (!_rolesCache.TryGetValue(userName,
                out (DateTime ExpiresAt, string Role) entry)
                || entry.ExpiresAt <= DateTime.Now)
            {

                var role = await (from r in appUserRolesDbContext.AppRoles
                                  join u in appUserRolesDbContext.AppUsers
                                      on r.Id equals u.RoleId
                                  where u.UserName == userName
                                  select r.RoleName).FirstOrDefaultAsync();

                if (role == default)
                    return "undefined"; //don't cache this

                entry = (DateTime.Now.AddMilliseconds(
                    _securityOptions.RefreshInterval), role);
                _rolesCache.AddOrUpdate(userName, entry, (u, e) => entry);
            }

            return entry.Role;
        }

    }

    public static class MvcAuthenticationProviderMiddlewareExtensions
    {
        public static IApplicationBuilder UseMvcAuthenticationProvider<TAppUserRolesDbContext>(this IApplicationBuilder app)
            where TAppUserRolesDbContext : AppUserRolesContextBase
        {
            app.UseMiddleware<MvcAuthenticationStateProviderMiddleware<TAppUserRolesDbContext>>();
            return app;
        }
    }
}
