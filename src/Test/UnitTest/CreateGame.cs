using CardGame.Core.Domain;
using CardGame.Core.Extensions;
using CardGame.Core.RequestResponse.CreateGame;
using CardGame.Core.Services.Implementations;
using CardGame.Core.UnitOfWork;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using TestProject.Infrastructures;

namespace TestProject;

public class CreateGame
{
    private readonly CreateGameRequestValidation _validation;

    public CreateGame()
    {
        _validation = new CreateGameRequestValidation();
    }
    

    [Fact]
    public void user_should_create_game()
    {

        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = Substitute.For<ICurrentUser>();
        var createGameService = new CreateGameService(repository, unitOfWork, currentUser);
        createGameService.CreateAsync(InitRequestModel.CreateGameRequest(), CancellationToken.None).Wait();
        repository.ReceivedCalls().Should().HaveCount(1);
        
        repository.Received().Add(Verify.That<Game>(game =>
        {
            game.IsFinished.Should().Be(false);
            game.IsStarted.Should().Be(false);
            game.GameBoards.Should().NotBeEmpty()
                .And.HaveCount(1)
            .And.Equal(new List<GameBoard>()
            {
                GameBoard.AddUserToBoard(true, InitRequestModel.CreateGameRequest().FullName)
            },(actual, expected) => actual.FullName == expected.FullName && actual.IsMain );
        }));
    }
    [Fact]
    public void Should_throw_an_exception_when_user_game_is_not_finished()
    {
        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = new FakeCurrentUser();
        
        var createGameService = new CreateGameService(repository, unitOfWork, currentUser);
        
        repository.GetById(1, CancellationToken.None).Returns(new Game());
       
        Action act = () =>  createGameService.CreateAsync(InitRequestModel.CreateGameRequest(), CancellationToken.None).Wait();
        act.Should().Throw<Exception>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_have_error_when_properties_are_not_filled(string fullName)
    {
        var request = new CreateGameRequest() {FullName = fullName};
        var result = _validation.TestValidate(request);
        result.ShouldHaveAnyValidationError();
    }
    


}