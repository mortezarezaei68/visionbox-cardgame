using CardGame.Core.Domain;
using CardGame.Core.Extensions;
using CardGame.Core.RequestResponse.CreateGame;
using CardGame.Core.UnitOfWork;
using FluentValidation;

namespace CardGame.Core.Services.Implementations;

public class CreateGameService : ICreateGameService
{
    private readonly IGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateGameService(IGameRepository repository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task CreateAsync(CreateGameRequest request, CancellationToken cancellationToken)
    {
        
        await Validation(cancellationToken);
        try
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var game = new Game(true, request.FullName);
            _repository.Add(game);
            await _unitOfWork.CommitAsync(transaction);

            _currentUser.SetHttpOnlyUserCookie(game.Id,
                game.GameBoards.FirstOrDefault(a => a.FullName == request.FullName)!.Id);
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            Console.WriteLine(e);
            throw;
        }
    }
    

    private async Task Validation(CancellationToken cancellationToken)
    {
        var gameId = _currentUser.GetCookieFromRequest(Constants.GameIdCookie);
        var gameBoardId = _currentUser.GetCookieFromRequest(Constants.GameBoardIdCookie);
        if (int.TryParse(gameId, out var id) || int.TryParse(gameBoardId, out _))
        {
            var game = await _repository.GetById(id, cancellationToken);
            if (!game.IsFinished || !game.GameBoards.FirstOrDefault(a => a.Id == int.Parse(gameBoardId))!.HasLeft)
                throw new Exception("currently game is continued you have to leave from that");

            if (game.IsFinished || game.GameBoards.FirstOrDefault(a => a.Id == int.Parse(gameBoardId)).HasLeft)
            {
                _currentUser.CleanSecurityCookie(Constants.GameIdCookie);
                _currentUser.CleanSecurityCookie(Constants.GameBoardIdCookie);
            }
        }
        
    }
}

public class CreateGameRequestValidation : AbstractValidator<CreateGameRequest>
{
    public CreateGameRequestValidation()
    {
        RuleFor(p => p.FullName).NotEmpty().NotNull();
    }
}