namespace CardGame.Core.Services;

public interface ILeftGameService
{
    Task LeftUser(CancellationToken cancellationToken);
}