using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [Header("Evolution Info")]
    [SerializeField] private Sprite _profileIcon = null;
    [SerializeField] private List<EvolutionLogic> _evolutionPaths;
    [SerializeField] private int _reqEndurance = 0;
    [SerializeField] private int _reqIntelligence = 0;
    [SerializeField] private int _reqStrength = 0;
    [SerializeField] private ChimeraType _myType;

    [Header("References")]
    [SerializeField] private Chimera _chimeraBrain;
    public void CheckEvolution(int endurance, int intelligence, int strength)
    {
        if(_evolutionPaths == null)
        {
            return;
        }

        foreach(var evolution in _evolutionPaths)
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

    private void Evolve(EvolutionLogic newModel)
    {
        Debug.Log("This creature is evolving into " + newModel + "!");

        EvolutionLogic newEvolution = Instantiate(newModel, _chimeraBrain.transform);
        _chimeraBrain.SetModel(newEvolution);
        Destroy(gameObject);
    }

    #region Getters & Setters
    public Sprite GetIcon() { return _profileIcon; }
    public int GetReqEnd() { return _reqEndurance; }
    public int GetReqInt() { return _reqIntelligence; }
    public int GetReqStr() { return _reqStrength; }
    public void SetChimeraBrain(Chimera chimera) { _chimeraBrain = chimera; }
    #endregion
    public ChimeraType GetChimeraType() { return _myType; }
}