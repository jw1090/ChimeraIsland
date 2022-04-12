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
    [SerializeField] private TextMeshProUGUI intelligence;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI happiness;
    [SerializeField] private TextMeshProUGUI element;
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

        //Debug.Log("Chimera: " + GameManager.Instance.GetActiveHabitat().GetChimeras()[chimeraSpot]);

        level.text = "Level: " + chimera.GetLevel();
        stamina.text = chimera.GetStatByType(StatType.Endurance).ToString();
        intelligence.text = chimera.GetStatByType(StatType.Intelligence).ToString();
        strength.text = chimera.GetStatByType(StatType.Strength).ToString();
        happiness.text = chimera.GetStatByType(StatType.Happiness).ToString();
        icon.SetTexture(chimera.GetIcon());
        element.text = "Element: " + chimera.GetElementalType().ToString();
    }

    public int GetChimeraSpot() { return chimeraSpot; }
}