using UnityEngine;

public class EnvironmentTier : MonoBehaviour
{
    [SerializeField] private FireflyFolder _fireflyFolder = null;

    public void ToggleFireflies(bool toggleOn)
    {
        if (_fireflyFolder.IsEmpty == true)
        {
            return;
        }

        if (toggleOn == true)
        {
            _fireflyFolder.PlayFireflies();
        }
        else
        {
            _fireflyFolder.StopFireflies();
        }
    }
}
