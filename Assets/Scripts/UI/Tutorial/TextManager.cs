using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo;
    private UIManager _uIManager;

    public TextManager Initialize()
    {
        _textInfo.Initialize();
        return _uIManager.GetTextManager();
    }
}
