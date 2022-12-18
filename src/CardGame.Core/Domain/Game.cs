using CardGame.Core.Enums;

namespace CardGame.Core.Domain;

public class Game : EntityAudit
{
    public int Id { get; private set; }
    public bool IsStarted { get; private set; }
    public bool IsFinished { get; private set; }
    public DateTime FinishedDate { get; private set; }
    public int Round { get; private set; }

    private readonly List<GameBoard> _gameBoards = new();
    public IReadOnlyCollection<GameBoard> GameBoards => _gameBoards;
    private readonly List<GivenCard> _givenCards = new();
    public List<GivenCard> GivenCards => _givenCards;

    public Game()
    {
        
    }

    public Game(bool isMain, string fullName)
    {
        _gameBoards.Add(GameBoard.AddUserToBoard(isMain, fullName));
    }

    public GameBoard UpdateUserList(string fullName)
    {

        var gameBoard = GameBoard.AddUserToBoard(false, fullName);

        _gameBoards.Add(gameBoard);

        return gameBoard;
    }

    public void GenerateNewCard(CardType cardType,int value)
    {
        if (_givenCards.Count > 53) return;

        if (_givenCards.Count is not 0)
        {
            var lastCard = _givenCards.OrderBy(a => a.CreatedAt).LastOrDefault(a => !a.HasGone);
            lastCard.UpdateGivenCardState();
        }

        var card = GivenCard.Add(cardType,
            value);
        
        _givenCards.Add(card);

    }

    public void UpdateRound()
    {
        Round += 1;
    }

    public void StartGame(CardType cardType,int value)
    {
        if (_gameBoards.OrderBy(a => a.CreatedAt).Any(a => IsStarted))
            throw new Exception("the game is started");

        _gameBoards.ForEach(board => { board.TurnOver(); });

        IsStarted = true;
        IsFinished = false;
        Round += 1;

        GenerateNewCard(cardType,value);

        var gameAdmin = GameBoards.FirstOrDefault(a => a.IsMain);

        gameAdmin.TakeTurn();
    }

    public void UpdateGameTurnBoard(GameBoard gameBoard)
    {
        gameBoard.TakeTurn();
    }
    

    public void UpdateCurrentlyRoundScore(int id)
    {
        var gameBoard = _gameBoards.FirstOrDefault(a => a.Id == id);
        if (gameBoard is null)
            throw new Exception("there is not any Main User In this GameBoard");

        gameBoard.UpdateDetailBoardScore(Round);
    }

    public void UpdateRoundScore(int gameBoardId)
    {
        var gameBoard = _gameBoards.FirstOrDefault(a => a.Id == gameBoardId);
        if (gameBoard is null)
            throw new Exception("there is not any Main User In this GameBoard");
        
        gameBoard.UpdateRoundScore();
    }

    public void LeftGameBoard(int id)
    {
        var gameBoard = _gameBoards.FirstOrDefault(a => a.Id == id);
        if (gameBoard is null)
            throw new Exception("there is not any Main User In this GameBoard");

        gameBoard.UpdateLeftState();
    }

    public void UpdateFinishedState()
    {
        IsFinished = true;
        FinishedDate = DateTime.Now;
    }
}