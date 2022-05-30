using UnityEngine;

public class SessionData : MonoBehaviour, ISessionData
{
    public Habitat CurrentHabitat { get; set; } = null;

    public SessionData Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");
        return this;
    }
}