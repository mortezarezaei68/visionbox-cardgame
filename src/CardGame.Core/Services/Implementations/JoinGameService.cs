using CardGame.Core.Domain;
using CardGame.Core.Extensions;
using CardGame.Core.RequestResponse.JoinGame;
using CardGame.Core.UnitOfWork;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace CardGame.Core.Services.Implementations;

public class JoinGameService : IJoinGameService
{
    private readonly IGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public JoinGameService(IGameRepository repository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task JoinGameAsync(JoinGameRequest request, CancellationToken cancellationToken)
    {
        var selectedGame = await _repository.GetById(request.GameId, cancellationToken);

        await Validation(cancellationToken, selectedGame);

        try
        {

            var transaction = await _unitOfWork.BeginTransactionAsync();
            var gameBoard = selectedGame.UpdateUserList(request.FullName);
            _repository.Update(selectedGame);
            await _unitOfWork.CommitAsync(transaction);

            _currentUser.SetHttpOnlyUserCookie(selectedGame.Id, gameBoard.Id);
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            Console.WriteLine(e);
            throw;
        }

      
    }

    private async Task Validation(CancellationToken cancellationToken, Game selectedGame)
    {
        if (selectedGame is {IsFinished: false, IsStarted: true})
            throw new Exception("currently game is started");

        var gameId = _currentUser.GetCookieFromRequest(Constants.GameIdCookie);
        var gameBoardId = _currentUser.GetCookieFromRequest(Constants.GameBoardIdCookie);
        if (int.TryParse(gameId, out var id) || int.TryParse(gameBoardId, out _))
        {
            var joinedGame = await _repository.GetById(id, cancellationToken);
            if (!joinedGame.IsFinished ||
                !joinedGame.GameBoards.FirstOrDefault(a => a.Id == int.Parse(gameBoardId))!.HasLeft)
                throw new Exception("currently game is continued you have to leave from that");
        }
        if (selectedGame.GameBoards.Count > 10)
            throw new Exception("game is full");
    }
}
public class JoinGameRequestValidation : AbstractValidator<JoinGameRequest>
{
    public JoinGameRequestValidation()
    {
        RuleFor(p => p.FullName).NotEmpty().NotNull();
        RuleFor(p => p.GameId).NotEmpty().NotNull();
    }
}