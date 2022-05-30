using UnityEngine;

public class FacilityIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _icon = null;

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
    public void RemoveIcon()
    {
        _icon.sprite = null;
    }
}