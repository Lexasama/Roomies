using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.Roomies.WebApp.Authentication
{
    public abstract class AuthenticationManager<TUserInfo>
    {
        public async Task OnCreatingTicket(OAuthCreatingTicketContext ctx)
        {
            TUserInfo userInfo = await GetUserInfoFromContext(ctx);
            await CreateOrUpdateUser(userInfo);
            RoomiesData roomie = await FindUser(userInfo);
            ctx.Principal = CreatePrincipal(roomie);
        }

        protected abstract Task<TUserInfo> GetUserInfoFromContext(OAuthCreatingTicketContext ctx);

        protected abstract Task CreateOrUpdateUser(TUserInfo userInfo);

        protected abstract Task<RoomiesData> FindUser(TUserInfo userInfo);

        ClaimsPrincipal CreatePrincipal(RoomiesData roomie)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier, roomie.RoomieId.ToString(), ClaimValueTypes.String ),
                new Claim( ClaimTypes.Email, roomie.Email)
            };
            ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthentication.AuthenticationType, ClaimTypes.Email, string.Empty));
            return principal;
        }
    }
}
