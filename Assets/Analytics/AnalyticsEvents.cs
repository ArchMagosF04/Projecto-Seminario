using Unity.Services.Analytics;

public class NewGameEvent : Event
{
    public NewGameEvent() : base("newGame")
    {
    }
}