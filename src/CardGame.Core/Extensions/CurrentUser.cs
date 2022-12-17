using CardGame.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CardGame.Core.Extensions
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public CurrentUser(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public void SetHttpOnlyUserCookie(int gameId, int gameBoardId)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(Constants.GameIdCookie, gameId.ToString(),
                new CookieOptions()
                {
                    HttpOnly = true, SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.Now.AddSeconds(int.Parse(_configuration["Game:ExpiredTime"])),
                    Domain = _configuration["Game:DomainUrl"], Secure = false
                });            
            
            _httpContextAccessor.HttpContext.Response.Cookies.Append(Constants.GameBoardIdCookie, gameBoardId.ToString(),
                new CookieOptions()
                {
                    HttpOnly = true, SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.Now.AddSeconds(int.Parse(_configuration["Game:ExpiredTime"])),
                    Domain = _configuration["Game:DomainUrl"], Secure = false
                });
        }

        public void CleanSecurityCookie(string key)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(key, new CookieOptions()
                {
                    Domain = _configuration["Game:DomainUrl"]
                });
        }

        public string GetCookieFromRequest(string key)
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(key);
            return cookieValue ? _httpContextAccessor.HttpContext.Request.Cookies[key] : null;
        }
    }
}