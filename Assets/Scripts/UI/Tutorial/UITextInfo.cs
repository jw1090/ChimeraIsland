using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tutorialText = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private float _speed = 0.01f;
    private bool _finished = true;
    private bool _finishNow = false;
    private string _text = "";
    private int _count = 0;
    private float _timePassed = 0.0f;
    private bool _isLoaded = false;

    public bool Finished { get => _finished; }

    public void Done() { _isLoaded = false; }

    public void FinishNow() { _finishNow = true; }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (_isLoaded == false)
        {
            return;
        }

        if (_finished == true)
        {
            return;
        }

        if (_finishNow == true)
        {
            _tutorialText.text = _text;
            _finished = true;
        }
        else
        {
            _timePassed += Time.deltaTime;
            if (_timePassed >= _speed)
            {
                ShowText(_count);
                _count++;
                _timePassed = 0;
            }
        }
    }

    public void Load(string Text, Sprite icon)
    {
        _isLoaded = true;
        _tutorialText.text = "";
        _icon.sprite = icon;
        _text = Text;
        _finishNow = false;
        _finished = false;
        _count = 1;
        _timePassed = 0;

        gameObject.SetActive(true);
    }


    private void ShowText(int length)
    {
        if (length >= _text.Length)
        {
            _finished = true;
            _tutorialText.text = _text;
            return;
        }

        string visible = _text.Substring(0, length);
        string notVisible = _text.Substring(length, _text.Length - (length + 1));
        _tutorialText.text = $"{visible}<color=#00000000>{notVisible}</color>";
    }
}