using UnityEngine;

public class SessionData : MonoBehaviour, ISessionData
{
    public string CurrentHabitat { get; set; }

    public SessionData Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} Initialized!</color>");
        return this;
    }
}
