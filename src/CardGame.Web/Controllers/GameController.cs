using CardGame.Core.RequestResponse.CardGame;
using CardGame.Core.RequestResponse.CreateGame;
using CardGame.Core.RequestResponse.JoinGame;
using CardGame.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardGame.Web.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GameController : ControllerBase
{
    private readonly ICreateGameService _createGameService;
    private readonly IJoinGameService _joinGameService;
    private readonly IGuessCardService _guessCardService;
    private readonly ILeftGameService _leftGameService;
    private readonly IGetGameListService _gameListService;
    private readonly IStartGameService _startGameService;

    public GameController(ICreateGameService createGameService, IJoinGameService joinGameService,
        IGuessCardService guessCardService, ILeftGameService leftGameService, IGetGameListService gameListService,
        IStartGameService startGameService)
    {
        _createGameService = createGameService;
        _joinGameService = joinGameService;
        _guessCardService = guessCardService;
        _leftGameService = leftGameService;
        _gameListService = gameListService;
        _startGameService = startGameService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateGameRequest request,
        CancellationToken cancellationToken = default)
    {
        await _createGameService.CreateAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> JoinAsync(JoinGameRequest request, CancellationToken cancellationToken = default)
    {
        await _joinGameService.JoinGameAsync(request, cancellationToken);
        return NoContent();
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> GuessCardAsync(GuessCardRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _guessCardService.GuessCardAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> LeftAsync(CancellationToken cancellationToken = default)
    {
        await _leftGameService.LeftUser(cancellationToken);
        return NoContent();
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> StartAsync(CancellationToken cancellationToken = default)
    {
        var result = await _startGameService.StartGameAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var result = await _gameListService.GetAllAsync(cancellationToken);
        return Ok(result);
    }
}