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
    [SerializeField] private float currentEssence = 0;
    [SerializeField] private float essenceModifier = 1.0f; // Tuning knob for essence gain
    [SerializeField] private int essenceCap = 100;

    [Header("Evolution Info")]
    [SerializeField] private Chimera[] evolutionPaths;
    [SerializeField] private int[] evolutionStats;

    // TODO: Complete ChimeraTick - Done ---> CHECK WITH JOE AND SANTIAGO IN CLASS !!!
    // - Called by the habitat to transfer habitat stat rates into the chimera's stored stats every tick
    // - Make sure each respective stat's experience does not go over the experienceCap variable
    public void ChimeraTick(int agility, int defense , int strength, int stamina, int wisdom)
    {
        //Checking if experience to be stored is within the experienceCap
        if(storedAgilityExperience <= experienceCap)
        {
            this.storedAgilityExperience += agility;
        }
        //Checking if experience to be stored is within the experienceCap
        if(storedDefenseExperience <= experienceCap)
        {
            this.storedDefenseExperience += defense;
        }
        //Checking if experience to be stored is within the experienceCap
        if(storedDefenseExperience <= experienceCap)
        {
            this.storedDefenseExperience += defense;
        }
        //Checking if experience to be stored is within the experienceCap
        if(storedStrenghtExperience <= experienceCap)
        {
            this.storedStrenghtExperience += strength;
        }
        //Checking if experience to be stored is within the experienceCap
        if(storedStaminaExperience <= experienceCap)
        {
            this.storedStaminaExperience += stamina;
        }
        //Checking if experience to be stored is within the experienceCap
        if(storedWisdomExperience <= experienceCap)
        {
            this.storedWisdomExperience += wisdom;
        }
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
        //TODO: REVIEW THE ESSENCE FORMULA IN CLASS TO CONSIDER Happiness PROPERLY ---> DISCUSS WITH JOE AND SANTIAGO IN CLASS !!!
        //Happiness for now = x1 let´s implement it properly in class given the workd done this week
        if(currentEssence <= essenceCap)
        {
             // make sure that essenceModifier is the variable intended here!!
            currentEssence += (level * 1 * baseEssenceRate) * essenceModifier;
            currentEssence = 0;
        }
        //The GameManager will only display this value to the player therefore it is ok casting it to int here.
        GameManager.Instance.IncreaseEssence( (int) currentEssence);
    }

    // TODO: Complete AllocateExperience
    // - Transfer experience stored by the chimera and see if each stat's threshold is met.
    // - If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        if(storedAgilityExperience >= agilityThreshold)
        {
            LevelUp(StatType.Agility);
            storedAgilityExperience = 0;
        }
        if(storedDefenseExperience >= defenceThreshold)
        {
            LevelUp(StatType.Defense);
            storedDefenseExperience = 0;
        }
        if(storedStrenghtExperience >= strengthThreshold)
        {
            LevelUp(StatType.Strength);
            storedStrenghtExperience = 0;
        }
        if(storedStaminaExperience >= staminaThreshold)
        {
            LevelUp(StatType.Stamina);
            storedStaminaExperience = 0;
        }
        if(storedWisdomExperience >= wisdomThreshold)
        {
            LevelUp(StatType.Wisdom);
            storedWisdomExperience = 0;
        }
    }

    // TODO: Complete LevelUp
    // - Increase stat at rate of the relevant statgrowth variable.
    // - For example: agility += agilityGrowth, defense += defenseGrowth...
    private void LevelUp(StatType statType)
    {
        switch (statType)
            {
                case StatType.Agility:
                    agility += agilityGrowth;
                    break;
                case StatType.Defense:
                    defense += defenseGrowth;
                    break;
                case StatType.Strength:
                    strength += strengthGrowth;
                    break;
                case StatType.Stamina:
                    stamina += staminaGrowth;
                    break;
                case StatType.Wisdom:
                    wisdom += wisdomGrowth;
                    break;
            }
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