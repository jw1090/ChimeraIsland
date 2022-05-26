using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [Header("Evolution Info")]
    [SerializeField] private ChimeraType _evolutionType;
    [SerializeField] private Sprite _icon = null;
    [SerializeField] private List<EvolutionLogic> _evolutionPaths;
    [SerializeField] private int _reqEndurance = 0;
    [SerializeField] private int _reqIntelligence = 0;
    [SerializeField] private int _reqStrength = 0;

    private Chimera _chimeraBrain = null;

    public ChimeraType Type { get => _evolutionType; }
    public Sprite Icon { get => _icon; }
    public int ReqEndurance { get => _reqEndurance; }
    public int ReqIntelligence { get => _reqIntelligence; }
    public int ReqStrength { get => _reqStrength; }

    public void SetChimeraBrain(Chimera chimera) { _chimeraBrain = chimera; }

    public bool CheckEvolution(int endurance, int intelligence, int strength)
    {
        if(_evolutionPaths == null)
        {
            return false;
        }

        foreach(var evolution in _evolutionPaths)
        {
            if (endurance < evolution.ReqEndurance)
            {
                continue;
            }
            else if (intelligence < evolution.ReqIntelligence)
            {
                continue;
            }
            if(strength < evolution.ReqStrength)
            {
                continue;
            }

            Evolve(evolution);
            return true;
        }

        return false;
    }

    private void Evolve(EvolutionLogic newModel)
    {
        Debug.Log("This creature is evolving into " + newModel + "!");

        EvolutionLogic newEvolution = Instantiate(newModel, _chimeraBrain.transform);
        _chimeraBrain.SetEvolutionLogic(newEvolution);
        _chimeraBrain.SetChimeraType(newEvolution.Type);

        Destroy(gameObject);
    }
}