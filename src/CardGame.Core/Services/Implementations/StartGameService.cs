using CardGame.Core.Domain;
using CardGame.Core.Extensions;
using CardGame.Core.RequestResponse.CardGame;
using CardGame.Core.RequestResponse.StartGame;
using CardGame.Core.UnitOfWork;

namespace CardGame.Core.Services.Implementations;

public class StartGameService:IStartGameService
{
    private readonly IGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;


    public StartGameService(IGameRepository repository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<StartGameResponse> StartGameAsync(CancellationToken cancellationToken)
    {
        var gameId = _currentUser.GetCookieFromRequest(Constants.GameIdCookie);
        var gameBoardId = _currentUser.GetCookieFromRequest(Constants.GameBoardIdCookie);
        if (gameId is null || gameBoardId is null)
            throw new Exception("you are not allowed to playing in the game");

        try
        {
            var game=await _repository.GetById(int.Parse(gameId), cancellationToken);

            if (game.GameBoards.Count<2)
                throw new Exception("the game has less than 2 user");
            
            var transaction = await _unitOfWork.BeginTransactionAsync();
            
            game.StartGame(GenerateCardRandomly.GetNextUniqueIntegerFunc(game.GivenCards).Invoke().cardType,
                GenerateCardRandomly.GetNextUniqueIntegerFunc(game.GivenCards).Invoke().value);
            
            _repository.Update(game);
            await _unitOfWork.CommitAsync(transaction);
            return new StartGameResponse
            {
                Value = game.GivenCards.FirstOrDefault(a => !a.HasGone).Value,
                CardType = game.GivenCards.FirstOrDefault(a => !a.HasGone).CardType,
                GameBoardId = game.GameBoards.FirstOrDefault(a=>a.IsMain).Id
            };
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            Console.WriteLine(e);
            throw;
        }
    }
}