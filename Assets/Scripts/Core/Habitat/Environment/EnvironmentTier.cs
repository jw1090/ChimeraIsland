using UnityEngine;

public class EnvironmentTier : MonoBehaviour
{
    [SerializeField] private GameObject _fireflyFolder = null;

    public void ToggleFireflies(bool toggleOn) { _fireflyFolder.SetActive(toggleOn); }
}
