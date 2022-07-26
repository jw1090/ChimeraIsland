using UnityEngine;

public class UIExpedition : MonoBehaviour
{
    [SerializeField] public ChimeraDetailsFolder _chimeraDetails = null;

    public void Initialization()
    {
        _chimeraDetails.Initialize();
    }
}