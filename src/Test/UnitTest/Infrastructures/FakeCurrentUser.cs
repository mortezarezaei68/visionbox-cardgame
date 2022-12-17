using CardGame.Core.Extensions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Constants = CardGame.Core.Services.Constants;

namespace TestProject.Infrastructures;

public class FakeCurrentUser:ICurrentUser
{
    public void SetHttpOnlyUserCookie(int gameId, int gameBoardId)
    {
        throw new NotImplementedException();
    }

    public void CleanSecurityCookie(string key)
    {
        throw new NotImplementedException();
    }

    public string GetCookieFromRequest(string key)
    {
        return "1";
    }
}