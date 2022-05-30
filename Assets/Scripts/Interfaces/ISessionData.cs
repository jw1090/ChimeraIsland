public interface ISessionData
{
    SessionData Initialize();
    Habitat CurrentHabitat { get; set; }
}