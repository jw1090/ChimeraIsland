using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraModel : MonoBehaviour
{
    [Header("Evolution Info")]
    [SerializeField] private Sprite profileIcon = null;
    [SerializeField] private List<ChimeraModel> evolutionPaths;
    [SerializeField] private int reqEndurance = 0;
    [SerializeField] private int reqIntelligence = 0;
    [SerializeField] private int reqStrength = 0;

    [Header("References")]
    [SerializeField] private Chimera chimera;

    private void Start()
    {
        chimera = GetComponentInParent<Chimera>();
    }

    public void CheckEvolution(int endurance, int intelligence, int strength)
    {
        if(evolutionPaths == null)
        {
            return;
        }

        foreach(var evolution in evolutionPaths)
        {
            if (endurance < evolution.GetReqEnd())
            {
                continue;
            }
            if (intelligence < evolution.GetReqInt())
            {
                continue;
            }
            if (strength < evolution.GetReqStr())
            {
                continue;
            }

            Evolve(evolution);
        }
    }

    private void Evolve(ChimeraModel newModel)
    {
        Debug.Log("This creature is evolving into " + newModel + "!");

        ChimeraModel newEvolution = Instantiate(newModel, chimera.transform);
        chimera.SetModel(newEvolution);
        Destroy(gameObject);
    }

    #region Getters & Setters
    public Sprite GetIcon() { return profileIcon; }
    public int GetReqEnd() { return reqEndurance; }
    public int GetReqInt() { return reqIntelligence; }
    public int GetReqStr() { return reqStrength; }
    #endregion
}