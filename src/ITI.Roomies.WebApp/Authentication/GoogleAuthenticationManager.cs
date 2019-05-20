using System.Threading.Tasks;
using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.Roomies.WebApp.Authentication
{
    public class GoogleAuthenticationManager : AuthenticationManager<GoogleUserInfo>
    {
        readonly RoomiesGateway _userGateway;

        public GoogleAuthenticationManager(RoomiesService userService, RoomiesGateway userGateway)
        {
            _userGateway = userGateway;
        }

        protected override async Task CreateOrUpdateUser(GoogleUserInfo userInfo)
        {
            if (userInfo.RefreshToken != null)
            {
                await _userGateway.CreateOrUpdateGoogleUser(userInfo.Email, userInfo.GoogleId, userInfo.RefreshToken);
            }
        }

        protected override Task<RoomiesData> FindUser(GoogleUserInfo userInfo)
        {
            return _userGateway.FindByGoogleId(userInfo.GoogleId);
        }

        protected override Task<GoogleUserInfo> GetUserInfoFromContext(OAuthCreatingTicketContext ctx)
        {
            return Task.FromResult(new GoogleUserInfo
            {
                RefreshToken = ctx.RefreshToken,
                Email = ctx.GetEmail(),
                GoogleId = ctx.GetGoogleId()
            });
        }
    }

    public class GoogleUserInfo
    {
        public string RefreshToken { get; set; }

        public string Email { get; set; }

        public string GoogleId { get; set; }
    }
}
