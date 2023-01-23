using CardGame.Core.Domain;
using CardGame.Core.Enums;
using CardGame.Core.Extensions;
using CardGame.Core.RequestResponse.CardGame;
using CardGame.Core.UnitOfWork;
using FluentValidation;

namespace CardGame.Core.Services.Implementations;

public class GuessCardService : IGuessCardService
{
    private readonly IGameRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private GuessCardResponse _guessCardResponse;
    private readonly IConfirmationCardService _confirmationCardService;

    public GuessCardService(ICurrentUser currentUser, IGameRepository repository, IUnitOfWork unitOfWork,
        IConfirmationCardService confirmationCardService)
    {
        _currentUser = currentUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _confirmationCardService = confirmationCardService;
        _guessCardResponse = new GuessCardResponse();
    }

    public async Task<GuessCardResponse> GuessCardAsync(GuessCardRequest request,
        CancellationToken cancellationToken)
    {
        var gameId = _currentUser.GetCookieFromRequest(Constants.GameIdCookie);
        var currentGameBoardIdCookie = _currentUser.GetCookieFromRequest(Constants.GameBoardIdCookie);

        Validation(gameId, currentGameBoardIdCookie);


        var game = await _repository.GetById(int.Parse(gameId), cancellationToken);

        ValidateUserTurn(game, currentGameBoardIdCookie);


        try
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();

            var card = GenerateCardRandomly.GetNextUniqueIntegerFunc(game.GivenCards).Invoke();
            game.GenerateNewCard(card.cardType, card.value);

            var nextGameBoard = game.GameBoards.OrderBy(a => a.CreatedAt).LastOrDefault(a => !a.IsTurn);

            _guessCardResponse=_confirmationCardService.IsConfirmedOrLoosed(request, currentGameBoardIdCookie, game, nextGameBoard?.Id);


            if (game.GameBoards.All(a => a.IsTurn))
            {
                FinishedGame(game, _guessCardResponse);
            }
            else
            {
                game.UpdateGameTurnBoard(nextGameBoard);
            }

            _repository.Update(game);
            await _unitOfWork.CommitAsync(transaction);
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            Console.WriteLine(e);
            throw;
        }


        return _guessCardResponse;
    }


    private static void ValidateUserTurn(Game game, string gameBoardId)
    {
        if (!game.GameBoards.FirstOrDefault(a => a.Id == int.Parse(gameBoardId)).IsTurn ||
            game.GameBoards.OrderBy(a => a.CreatedAt).LastOrDefault(a => a.IsTurn).Id != int.Parse(gameBoardId) ||
            game.IsFinished)
            throw new Exception("is it not your turn");
    }


    private void FinishedGame(Game game, GuessCardResponse? currentGameResponse)
    {
        foreach (var gameGameBoard in game.GameBoards)
        {
            if (gameGameBoard.BoardDetails.Count != 0 &&
                gameGameBoard.BoardDetails.FirstOrDefault(a => a.Round == game.Round)?.RoundScore > 0)
            {
                game.UpdateRoundScore(gameGameBoard.Id);
            }
        }
        if (game.GivenCards.Count == 53)
        {
            game.UpdateFinishedState();
            currentGameResponse.IsFinished = true;
        }
        else
        {
            foreach (var gameGameBoard in game.GameBoards)
                if (!gameGameBoard.IsMain)
                    gameGameBoard.TurnOver();
            currentGameResponse.RoundFinished = true;
            game.UpdateRound();
        }




        currentGameResponse.ScoreResponses = game.GameBoards.Select(a => new ScoreResponse
        {
            FullName = a.FullName,
            Score = a.Score
        }).ToList();
    }

    private static void Validation(string gameIdCookie, string gameBoardIdCookie)
    {
        if (gameIdCookie is null || gameBoardIdCookie is null)
            throw new Exception("you are not allowed to playing in the game");
    }
}

public class GuessCardRequestValidation : AbstractValidator<GuessCardRequest>
{
    public GuessCardRequestValidation()
    {
        RuleFor(p => p.GuessType).NotNull();
    }
}