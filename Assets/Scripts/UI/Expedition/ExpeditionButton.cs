using UnityEngine;
using UnityEngine.UI;

public class ExpeditionButton : MonoBehaviour
{
    [SerializeField] GameObject _notification = null;
    [SerializeField] Button _button = null;

    public Button Button { get => _button; }

    public void ActivateNotification(bool activate)
    {
        _notification.SetActive(activate);
    }
}