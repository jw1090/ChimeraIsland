using UnityEngine;
using TMPro;

public class ChimeraDetails : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int chimeraSpot;
    [SerializeField] private Chimera chimera;
    [SerializeField] private CanvasRenderer icon;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI intelligence;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI happiness;
    [SerializeField] private TextMeshProUGUI element;

    void OnEnable()
    {
        UpdateDetails();
    }

    private void Update()
    {
        //UpdateDetails();
    }

    public void UpdateDetails()
    {
        if (GameManager.Instance.GetActiveHabitat().GetChimeras().Count <= chimeraSpot)
        {
            gameObject.SetActive(false);
            return;
        }

        chimera = GameManager.Instance.GetActiveHabitat().GetChimeras()[chimeraSpot];

        //Debug.Log("Chimera: " + GameManager.Instance.GetActiveHabitat().GetChimeras()[chimeraSpot]);
        //Debug.Log("Icon Image: " + chimera.GetIcon());

        level.text = "Level: " + chimera.GetLevel();
        stamina.text = chimera.GetStatByType(StatType.Endurance).ToString();
        intelligence.text = chimera.GetStatByType(StatType.Intelligence).ToString();
        strength.text = chimera.GetStatByType(StatType.Strength).ToString();
        happiness.text = chimera.GetStatByType(StatType.Happiness).ToString();
        element.text = "Element: " + chimera.GetElementalType().ToString();
        icon.SetTexture(chimera.GetIcon());
    }

    public int GetChimeraSpot() { return chimeraSpot; }
}