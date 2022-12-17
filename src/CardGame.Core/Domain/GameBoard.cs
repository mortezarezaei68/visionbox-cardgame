namespace CardGame.Core.Domain;

public class GameBoard : EntityAudit
{
    public static GameBoard AddUserToBoard(bool isMain, string fullName)
    {
        return new GameBoard
        {
            IsMain = isMain,
            FullName = fullName,

        };
    }

    public void UpdateDetailBoardScore(int round)
    {
        var scoreInRound = _boardDetails.FirstOrDefault(a=>a.Round==round);
        if (_boardDetails.Count==0)
        {
            _boardDetails.Add(new BoardDetail(round));
        }
        else
        {
            scoreInRound.UpdateScore();
        }
    }

    public void UpdateRoundScore()
    {
        Score += 1;
    }

    public void TakeTurn()
    {
        IsTurn = true;
    }
    public void TurnOver()
    {
        IsTurn = false;
    }

    public void UpdateLeftState()
    {
        HasLeft = true;
    }

    public int Id { get; set; }
    public bool IsMain { get; private set; }
    public string FullName { get; private set; }
    public int Score { get; private set; }
    public int GameId { get; private set; }
    public bool HasLeft { get; private set; }
    public bool IsTurn { get; private set; }
    private readonly List<BoardDetail> _boardDetails = new();

 

    public IReadOnlyCollection<BoardDetail> BoardDetails => _boardDetails;
}