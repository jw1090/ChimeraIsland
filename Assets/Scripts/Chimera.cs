using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType chimeraType = ChimeraType.None;
    [SerializeField] private ElementalType elementalType = ElementalType.None;
    [SerializeField] private Habitat parentHabitat;

    [Header("Stats")]
    [SerializeField] private int level;
    [SerializeField] private int agility;
    [SerializeField] private int strength;
    [SerializeField] private int defense;
    [SerializeField] private int stamina;
    [SerializeField] private int wisdom;
    [SerializeField] private int happiness;

    [Header("Stat Growth")]
    [SerializeField] private int agilityGrowth = 1;
    [SerializeField] private int defenseGrowth = 1;
    [SerializeField] private int strengthGrowth = 1;
    [SerializeField] private int staminaGrowth = 1;
    [SerializeField] private int wisdomGrowth = 1;
    [SerializeField] private int agilityExperience = 0;
    [SerializeField] private int defenseExperience = 0;
    [SerializeField] private int strengthExperience = 0;
    [SerializeField] private int staminaExperience = 0;
    [SerializeField] private int wisdomExperience = 0;
    [SerializeField] private int agilityThreshold = 100;
    [SerializeField] private int defenceThreshold = 100;
    [SerializeField] private int strengthThreshold = 100;
    [SerializeField] private int staminaThreshold = 100;
    [SerializeField] private int wisdomThreshold = 100;

    [Header("Stored Experience")]
    [SerializeField] private int storedAgilityExperience = 0;
    [SerializeField] private int storedDefenseExperience = 0;
    [SerializeField] private int storedStrenghtExperience = 0;
    [SerializeField] private int storedStaminaExperience = 0;
    [SerializeField] private int storedWisdomExperience = 0;
    [SerializeField] private int experienceCap = 200;

    [Header("Essence")]
    [SerializeField] private float baseEssenceRate = 5;
    [SerializeField] private int currentEssence = 0;
    [SerializeField] private float essenceModifier = 1.0f; // Tuning knob for essence gain
    [SerializeField] private int essenceCap = 100;

    [Header("Evolution Info")]
    [SerializeField] private Chimera[] evolutionPaths;
    [SerializeField] private int[] evolutionStats;

    // TODO: Complete ChimeraTick
    // - Called by the habitat to transfer habitat stat rates into the chimera's stored stats every tick
    // - Make sure each respective stat's experience does not go over the experienceCap variable
    public void ChimeraTick(int agility, int defense , int strength, int stamina, int wisdom)
    {

    }

    //  - Made by: Joe 2/2/2022
    //  - On tap call HarvestEssence() and AllocateExperience() functions to appropritely gain resources that have been stored
    //  - Any other on tap interaction will go in here
    public void ChimeraTap()
    {
        HarvestEssence();
        AllocateExperience();
    }

    // TODO: Complete HarvestEssence
    // - This function is called by ChimeraTap(). On tap it will use essence forumala
    // - and add stored essence to GameManager.Instance().IncreaseEssence(storedEssence)
    // - currentEssence += (( x )Level * ( x )HappinessFormula * MonsterBaseValue) * essenceModifier 
    // - Happiness for now = x1
    private void HarvestEssence()
    {

    }

    // TODO: Complete AllocateExperience
    // - Transfer experience stored by the chimera and see if each stat's threshold is met.
    // - If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {

    }

    // TODO: Complete LevelUp
    // - Increase stat at rate of the relevant statgrowth variable.
    // - For example: agility += agilityGrowth, defense += defenseGrowth...
    private void LevelUp(StatType statType)
    {

    }

    // - Check if all requirements are ok to evolve
    private void CheckEvolution()
    {

    }

    // - Evolve Chimera to its new form
    private void Evolve(Chimera newForm)
    {

    }

    #region Getters & Setters
    // Get the required stats needed to evolve
    public int[] GetRequiredStats() { return evolutionStats; }
    #endregion
}