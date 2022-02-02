using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chimera : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private ChimeraType chimeraType = ChimeraType.None;
    [SerializeField] private ElementalType elementalType;
    [SerializeField] private BehaviorType behaviorType;
    [SerializeField] private Habitat parentHabitat;
    [SerializeField] private int baseEssenceRate;
    [SerializeField] private int happiness;
    [SerializeField] private int agility;
    [SerializeField] private int strength;
    [SerializeField] private int defense;
    [SerializeField] private int stamina;
    [SerializeField] private int wisdom;

    [Header("Stat Growth")]
    [SerializeField] private int agilityGrowth;
    [SerializeField] private int strengthGrowth;
    [SerializeField] private int defenseGrowth;
    [SerializeField] private int staminaGrowth;
    [SerializeField] private int wisdomGrowth;
    [SerializeField] private int agilityExperience;
    [SerializeField] private int strengthExperience;
    [SerializeField] private int defenseExperience;
    [SerializeField] private int staminaExperience;
    [SerializeField] private int wisdomExperience;
    [SerializeField] private int agilityThreshold;
    [SerializeField] private int strengthThreshold;
    [SerializeField] private int defenceThreshold;
    [SerializeField] private int staminaThreshold;
    [SerializeField] private int wisdomThreshold;

    [Header("Stored Stat Experience")]
    [SerializeField] private int storedAgilityExperience = 0;
    [SerializeField] private int storedStrenghtExperience = 0;
    [SerializeField] private int storedDefenseExperience = 0;
    [SerializeField] private int storedStaminaExperience = 0;
    [SerializeField] private int storedWisdomExperience = 0;

    [Header("Evolution Info")]
    [SerializeField] private Chimera[] evolutionPaths;
    [SerializeField] private int evolutionStats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: Complete this function
    //  - Increase stat at rate of the relevant statgrowth variable agility += agilityGrowth
    //  - Made by: Joao 2/2/2022
    //  - Level Chimera up
    private void LevelUp(StatType statType)
    {

    }

    //  - Made by: Joao 2/2/2022
    //  - Check if all requirements are ok to evolve
    private void CheckEvolution()
    {

    }

    //  - Made by: Joao 2/2/2022
    //  - Evolve Chimera to its new form
    public void Evolve(Chimera newForm)
    {

    }

    // TODO: Complete this function
    // - Transfer Experience stored to chimera and see if threshold is met.
    // - If so, LevelUp is called with specific stat enumerator.
    public void ChimeraTap()
    {

    }

    // TODO: Complete this function
    //  - Called by the habitat to transfer habitat stat rates into the chimera's stored stats every tick
    //  - Made by: Joao 2/2/2022
    //  - Stored expereince on habitat tick
    public void TickExperience(int agility, int strength, int defense, int stamina, int wisdom)
    {
        
    }

    //  - Made by: Joao 2/2/2022
    //  - Get the required stats needed to evolve
    public int [] GetRequiredStats()
    {
        int[] requirements = { };
        return requirements;
    }

    // TODO: Complete this function
    //  - Use essence forumal to create this getter function
    //  - currentEssence += (( x )Level * ( x )HappinessFormula * MonsterBaseValue) *  essenceRatio 
    //  - Happiness for now = x1
    //  - Made by: Joao 2/2/2022
    //  - Get the essence when tapped. It is going to be used in habitats in order to update it with facilities.
    public int GetEssence()
    {
        return 0;
    }
}
