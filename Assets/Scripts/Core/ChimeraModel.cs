using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraModel : MonoBehaviour
{
    [Header("Egg Info")]
    [SerializeField] private bool isEgg = false;

    [Header("Evolution Info")]
    [SerializeField] private List<ChimeraModel> evolutionPaths;
    [SerializeField] private int reqIntelligence = 0;
    [SerializeField] private int reqStamina = 0;
    [SerializeField] private int reqStrength = 0;

    [Header("References")]
    [SerializeField] private Chimera chimera;

    private void Start()
    {
        chimera = GetComponentInParent<Chimera>();
    }

    public void CheckEvolution(int intelligence, int stamina, int strength)
    {
        if(evolutionPaths == null)
        {
            return;
        }

        foreach(var evolution in evolutionPaths)
        {
            if (intelligence < evolution.GetReqInt())
            {
                continue;
            }
            if (stamina < evolution.GetReqStam())
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
    public int GetReqInt() { return reqIntelligence; }
    public int GetReqStam() { return reqStamina; }
    public int GetReqStr() { return reqStrength; }
    #endregion
}