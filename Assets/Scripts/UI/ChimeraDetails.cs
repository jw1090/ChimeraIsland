using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChimeraDetails : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int chimeraSpot;
    [SerializeField] private CanvasRenderer icon;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI agility;
    [SerializeField] private TextMeshProUGUI defence;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI wisdom;
    [SerializeField] private TextMeshProUGUI happiness;
    private ChimeraDetailsFolder chimeraDetailsFolder;

    private void Start()
    {
        UpdateDetails();

        chimeraDetailsFolder = GetComponentInParent<ChimeraDetailsFolder>();
        chimeraDetailsFolder.Subscribe(this);
    }

    private void Update()
    {
        UpdateDetails();
    }

    private void UpdateDetails()
    {
        List<Chimera> chimera = GameManager.Instance.GetActiveHabitat().GetChimeras();

        level.text = "Level: " + chimera[chimeraSpot].GetLevel();
        agility.text = chimera[chimeraSpot].GetStatByType(StatType.Agility).ToString();
        defence.text = chimera[chimeraSpot].GetStatByType(StatType.Defense).ToString();
        stamina.text = chimera[chimeraSpot].GetStatByType(StatType.Stamina).ToString();
        strength.text = chimera[chimeraSpot].GetStatByType(StatType.Strength).ToString();
        wisdom.text = chimera[chimeraSpot].GetStatByType(StatType.Wisdom).ToString();
        happiness.text = chimera[chimeraSpot].GetStatByType(StatType.Happiness).ToString();
    }

    private int GetChimeraSpot() { return chimeraSpot; }
}