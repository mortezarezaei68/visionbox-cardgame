using CardGame.Core.Domain;
using CardGame.Core.Extensions;
using CardGame.Core.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace CardGame.Core.Services.Implementations;

public class LeftGameService:ILeftGameService
{
    private readonly ICurrentUser _currentUser;
    private readonly IConfiguration _configuration;
    private readonly IGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;


    public LeftGameService(ICurrentUser currentUser, IConfiguration configuration, IGameRepository repository, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _configuration = configuration;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task LeftUser(CancellationToken cancellationToken)
    {
        var gameId = _currentUser.GetCookieFromRequest(Constants.GameIdCookie);
        var gameBoardId = _currentUser.GetCookieFromRequest(Constants.GameBoardIdCookie);
        if (gameId is null && gameBoardId is null)
            throw new Exception("you are not playing any game");
        try
        {
            var game=await _repository.GetById(int.Parse(gameId), cancellationToken);
            var transaction = await _unitOfWork.BeginTransactionAsync();
            game.LeftGameBoard(int.Parse(gameBoardId));
            _repository.Update(game);
            await _unitOfWork.CommitAsync(transaction);
        
            _currentUser.CleanSecurityCookie(Constants.GameIdCookie );
            _currentUser.CleanSecurityCookie(Constants.GameBoardIdCookie );
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            Console.WriteLine(e);
            throw;
        }
    }
}