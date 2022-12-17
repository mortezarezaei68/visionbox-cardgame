namespace CardGame.Core.RequestResponse.GetGame;

public class GetGameResponse
{
    public string FullName { get; set; }
    public int Id { get; set; }
    public bool IsStartingState { get; set; }
}