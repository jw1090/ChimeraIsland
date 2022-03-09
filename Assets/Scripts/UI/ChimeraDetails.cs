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
    private Chimera chimera;

    private void Start()
    {
        chimeraDetailsFolder = GetComponentInParent<ChimeraDetailsFolder>();
    }

    private void Update()
    {
        UpdateDetails();
    }

    private void UpdateDetails()
    {
        if(GameManager.Instance.GetActiveHabitat().GetChimeras().Count <= chimeraSpot)
        {
            gameObject.SetActive(false);
            return;
        }

        chimera = GameManager.Instance.GetActiveHabitat().GetChimeras()[chimeraSpot];

        level.text = "Level: " + chimera.GetLevel();
        agility.text = chimera.GetStatByType(StatType.Agility).ToString();
        defence.text = chimera.GetStatByType(StatType.Defense).ToString();
        stamina.text = chimera.GetStatByType(StatType.Stamina).ToString();
        strength.text = chimera.GetStatByType(StatType.Strength).ToString();
        wisdom.text = chimera.GetStatByType(StatType.Wisdom).ToString();
        happiness.text = chimera.GetStatByType(StatType.Happiness).ToString();
        icon.SetTexture(chimera.GetProfileIcon());
    }

    public int GetChimeraSpot() { return chimeraSpot; }
}