using CardGame.Core.Domain;
using CardGame.Core.Extensions;
using CardGame.Core.RequestResponse.JoinGame;
using CardGame.Core.Services.Implementations;
using CardGame.Core.UnitOfWork;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using TestProject.Infrastructures;

namespace TestProject;

public class JoinGame
{
    private readonly JoinGameRequestValidation _validation;

    public JoinGame()
    {
        _validation = new JoinGameRequestValidation();
    }

    [Fact]
    public void user_should_join_game()
    {
        var game = new Game(true, "AdminTest");
        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = Substitute.For<ICurrentUser>();
        var joinGameService = new JoinGameService(repository, unitOfWork, currentUser);
        repository.GetById(1, CancellationToken.None).Returns(game);
        joinGameService.JoinGameAsync(InitRequestModel.JoinGameRequest(), CancellationToken.None).Wait();

        repository.Received().Update(Verify.That<Game>(game =>
        {
            game.IsFinished.Should().Be(false);
            game.IsStarted.Should().Be(false);
            game.GameBoards.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.Contain(a => a.FullName == InitRequestModel.JoinGameRequest().FullName && !a.IsMain);
        }));
    }

    [Fact]
    public void user_should_not_be_greater_than_10_users()
    {
        var game = new Game(true, "AdminTest");

        for (var i = 0; i < 10; i++)
        {
            game.UpdateUserList(NSubstituteExtensions.RandomString(5));
        }


        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = Substitute.For<ICurrentUser>();
        var joinGameService = new JoinGameService(repository, unitOfWork, currentUser);
        repository.GetById(1, CancellationToken.None).Returns(game);

        Action act = () =>
            joinGameService.JoinGameAsync(InitRequestModel.JoinGameRequest(), CancellationToken.None).Wait();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void user_join_to_other_games_should_throw_an_exception()
    {
        var game = new Game(true, "AdminTest");
        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = new FakeCurrentUser();
        var joinGameService = new JoinGameService(repository, unitOfWork, currentUser);
        repository.GetById(1, CancellationToken.None).Returns(game);

        Action act = () =>
            joinGameService.JoinGameAsync(InitRequestModel.JoinGameRequest(), CancellationToken.None).Wait();
        act.Should().Throw<Exception>();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", 0)]
    public void Should_have_error_when_properties_are_not_filled(string fullName, int gameId)
    {
        var request = new JoinGameRequest() {FullName = fullName, GameId = gameId};
        var result = _validation.TestValidate(request);
        result.ShouldHaveAnyValidationError();
    }
}