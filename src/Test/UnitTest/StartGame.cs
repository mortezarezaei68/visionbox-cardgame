using CardGame.Core.Domain;
using CardGame.Core.Enums;
using CardGame.Core.Extensions;
using CardGame.Core.Services.Implementations;
using CardGame.Core.UnitOfWork;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TestProject.Infrastructures;

namespace TestProject;

public class StartGame
{
    [Fact]
    public void user_should_start_game()
    {
        var game = new Game(true,"AdminTest");
        game.UpdateUserList("SubUserTest1");
        
        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = new FakeCurrentUser();
        var startGameService = new StartGameService(repository, unitOfWork, currentUser);
        repository.GetById(1, CancellationToken.None).Returns(game);
        
        var result= startGameService.StartGameAsync(CancellationToken.None).Result;
        
        repository.Received().Update(Verify.That<Game>(game =>
        {
            game.IsFinished.Should().Be(false);
            game.IsStarted.Should().Be(true);
            game.GivenCards.Should().HaveCountGreaterThanOrEqualTo(1);
            game.GameBoards.Should().NotBeEmpty()
                .And.HaveCountGreaterThanOrEqualTo(2).And.HaveCountLessThanOrEqualTo(10)
                .And.Contain(a => a.IsMain && a.IsTurn);
        }));
        Enum.IsDefined(result.CardType).Should().Be(true);
        result.Value.Should().BeInRange(1, 13);
    }
    [Fact]
    public void game_should_not_start_by_one_user()
    {
        var game = new Game(true,"AdminTest");
        
        var repository = Substitute.For<IGameRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = new FakeCurrentUser();
        var startGameService = new StartGameService(repository, unitOfWork, currentUser);
        repository.GetById(1, CancellationToken.None).Returns(game);
        
        Action act = () =>  startGameService.StartGameAsync(CancellationToken.None).Wait();
        act.Should().Throw<Exception>();
    }
    
}