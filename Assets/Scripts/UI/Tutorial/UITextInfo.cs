using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tutorialText = null;
    [SerializeField] private Image _icon = null;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Load(string text, Sprite icon)
    {
        _tutorialText.text = text;
        _icon.sprite = icon;
        gameObject.SetActive(true);
    }
}