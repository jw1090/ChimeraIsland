using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingChimeraInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _chimeraName = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private List<GameObject> _explorationPreference = new List<GameObject>();
    [SerializeField] private List<GameObject> _staminaPreference = new List<GameObject>();
    [SerializeField] private List<GameObject> _wisdomPreference = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI _chimeraInfo = null;

    public void LoadChimeraData(string name, ElementType iconType, int explorationPreference, int staminaPreference, int wisdomPreference, string info)
    {

    }
}