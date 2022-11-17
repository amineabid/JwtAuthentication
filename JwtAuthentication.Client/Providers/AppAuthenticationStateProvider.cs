using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtAuthentication.Client.Providers
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService; 
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler =new JwtSecurityTokenHandler();
        public AppAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string saveToken = await _localStorageService.GetItemAsync<string>("barerToken");
                if (string.IsNullOrWhiteSpace(saveToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                JwtSecurityToken jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(saveToken);
                DateTime expiry = jwtSecurityToken.ValidTo;
                if (expiry<DateTime.UtcNow)
                {
                    await _localStorageService.RemoveItemAsync("barerToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                }
                var claims = ParseClaims(jwtSecurityToken);
                var user =  new ClaimsPrincipal(new ClaimsIdentity(claims,"jwt"));
                return new AuthenticationState(user);
            }
            catch (Exception)
            {
                return new AuthenticationState ( new ClaimsPrincipal(new ClaimsIdentity())) ;
            }
        }

        private IList<Claim> ParseClaims(JwtSecurityToken jwtSecurityToken)
        {
            IList<Claim> claims = jwtSecurityToken.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name,jwtSecurityToken.Subject));
            return claims;
        }
        internal void SignOut()
        {
            ClaimsPrincipal nobody = new ClaimsPrincipal(new ClaimsIdentity());
            Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authenticationState);
        }
        internal async Task SignIn()
        {
            string saveToken = await _localStorageService.GetItemAsync<string>("barerToken");
            JwtSecurityToken jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(saveToken);
            var claims = ParseClaims(jwtSecurityToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authenticationState);
        }
    }
}
