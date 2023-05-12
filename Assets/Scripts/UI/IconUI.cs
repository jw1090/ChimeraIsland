using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    [SerializeField] private Image _icon = null;

    public void ToggleIcon(bool toggle)
    {
        _icon.gameObject.SetActive(toggle);
    }

    public void UpdateSprite(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}