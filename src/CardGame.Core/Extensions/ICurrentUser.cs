namespace CardGame.Core.Extensions
{
    public interface ICurrentUser
    {
        void SetHttpOnlyUserCookie(int gameId, int gameBoardId);
        void CleanSecurityCookie(string key);
        string GetCookieFromRequest(string key);
    }
}