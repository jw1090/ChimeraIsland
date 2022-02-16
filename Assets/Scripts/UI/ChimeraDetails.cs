using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChimeraDetails : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Chimera chimera;
    [SerializeField] private CanvasRenderer icon;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI agility;
    [SerializeField] private TextMeshProUGUI defence;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI wisdom;
    [SerializeField] private TextMeshProUGUI happiness;

    private void Start()
    {
        UpdateDetails();
    }

    private void Update()
    {
        UpdateDetails();
    }

    private void UpdateDetails()
    {
        level.text = "Level: " + chimera.GetStatByType(StatType.Agility).ToString();
        agility.text = chimera.GetStatByType(StatType.Agility).ToString();
        defence.text = chimera.GetStatByType(StatType.Agility).ToString();
        stamina.text = chimera.GetStatByType(StatType.Agility).ToString();
        strength.text = chimera.GetStatByType(StatType.Agility).ToString();
        wisdom.text = chimera.GetStatByType(StatType.Agility).ToString();
        happiness.text = chimera.GetStatByType(StatType.Agility).ToString();
    }
}