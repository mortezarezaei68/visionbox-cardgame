using CardGame.Core.Domain;
using CardGame.Core.Enums;
using CardGame.Core.Extensions;
using CardGame.Core.RequestResponse.CardGame;
using CardGame.Core.Services;
using CardGame.Core.Services.Implementations;
using CardGame.Core.UnitOfWork;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using TestProject.Infrastructures;

namespace TestProject;

public class GuessCard
{
    private readonly GuessCardRequestValidation _validation;

    public GuessCard()
    {
        _validation = new GuessCardRequestValidation();
    }

    [Fact]
    public void user_should_validate_turn()
    {
        var game = InitRequestModel.InitiateGameForValidateUserTurn();

        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var confirmationCardService = Substitute.For<IConfirmationCardService>();
        var currentUser = new FakeCurrentUser();
        var guessCardService = new GuessCardService(currentUser, repository, unitOfWork, confirmationCardService);
        repository.GetById(1, CancellationToken.None).Returns(game);

        Action act = () =>
            guessCardService.GuessCardAsync(InitRequestModel.CreateGuessCardRequest(), CancellationToken.None).Wait();
        act.Should().Throw<Exception>();
    }

 

    [Fact]
    public void confirmed_user_card()
    {
        var game=InitRequestModel.InitiateGameForConfirmCard();
        var currentUser = new FakeCurrentUser();
        var isConfirmedOrLoosed = new ConfirmationCardService();
        var response = isConfirmedOrLoosed.IsConfirmedOrLoosed(InitRequestModel.CreateGuessCardRequest(),
            currentUser.GetCookieFromRequest(Constants.GameIdCookie), game, 2);
        response.Value.Should().BeInRange(1, 13);
        Enum.IsDefined(response.CardType).Should().Be(true);
        response.IsFinished.Should().Be(false);

        game.GameBoards.First().BoardDetails.Count.Should().BeGreaterThan(0);
    }
    
}