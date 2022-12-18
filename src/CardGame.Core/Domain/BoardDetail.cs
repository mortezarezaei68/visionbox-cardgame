namespace CardGame.Core.Domain;

public class BoardDetail:EntityAudit
{
    public int RoundScore { get; private set; }
    public int Round { get; private set; }
    public int Id { get; set; }
    public BoardDetail(int round)
    {
        Round = round;
        RoundScore += 1;
    }

    public void UpdateScore()
    {
        RoundScore++;
    }


}