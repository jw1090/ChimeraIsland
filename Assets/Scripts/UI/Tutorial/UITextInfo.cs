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
                ShowText();
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


    private void ShowText()
    {
        if (_count >= _text.Length)
        {
            _finished = true;
            _tutorialText.text = _text;
            return;
        }
        if (_text.Substring(_count-1,1).EndsWith("<"))
        {
            while(true)
            {
                if (_text.Substring(_count, 1).EndsWith(">"))
                {
                    _count++;
                    break;
                }
                _count++;
            }
        }
        string visible = _text.Substring(0, _count);
        string notVisible = _text.Substring(_count, Mathf.Max(_text.Length - (_count + 1),0));
        notVisible = notVisible.Replace("<sprite name=fira>", "X");
        notVisible = notVisible.Replace("<sprite name=bio>", "X");
        notVisible = notVisible.Replace("<sprite name=aqua>", "X");
        notVisible = notVisible.Replace("<sprite name=Wisdom>", "X");
        notVisible = notVisible.Replace("<sprite name=Exploration>", "X");
        notVisible = notVisible.Replace("<sprite name=Stamina>", "X");
        _tutorialText.text = $"{visible}<color=#00000000>{notVisible}</color>";
    }
}