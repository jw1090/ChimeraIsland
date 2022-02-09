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
    [SerializeField] private bool tappable = false;

    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private int agility = 0;
    [SerializeField] private int strength = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private int stamina = 0;
    [SerializeField] private int wisdom = 0;
    [SerializeField] private int happiness = 0;

    [Header("Stat Growth")]
    [SerializeField] private int agilityGrowth = 1;
    [SerializeField] private int defenseGrowth = 1;
    [SerializeField] private int staminaGrowth = 1;
    [SerializeField] private int strengthGrowth = 1;
    [SerializeField] private int wisdomGrowth = 1;
    [SerializeField] private int agilityExperience = 0;
    [SerializeField] private int defenseExperience = 0;
    [SerializeField] private int staminaExperience = 0;
    [SerializeField] private int strengthExperience = 0;
    [SerializeField] private int wisdomExperience = 0;
    [SerializeField] private int agilityThreshold = 100;
    [SerializeField] private int defenceThreshold = 100;
    [SerializeField] private int staminaThreshold = 100;
    [SerializeField] private int strengthThreshold = 100;
    [SerializeField] private int wisdomThreshold = 100;

    [Header("Stored Experience")]
    [SerializeField] private int storedAgilityExperience = 0;
    [SerializeField] private int storedDefenseExperience = 0;
    [SerializeField] private int storedStaminaExperience = 0;
    [SerializeField] private int storedStrengthExperience = 0;
    [SerializeField] private int storedWisdomExperience = 0;
    [SerializeField] private int experienceCap = 200;

    [Header("Essence")]
    [SerializeField] private float baseEssenceRate = 5;
    [SerializeField] private int storedEssence = 0;
    [SerializeField] private float essenceModifier = 1.0f; // Tuning knob for essence gain
    [SerializeField] private int essenceCap = 100;

    [Header("Evolution Info")]
    [SerializeField] private Chimera[] evolutionPaths;
    [SerializeField] private int[] evolutionStats;

    [Header("Debug Materials")]
    [SerializeField] MeshRenderer model;
    [SerializeField] Material standardMat;
    [SerializeField] Material tappableMat;

    // - Made by: Joe 2/9/2022
    // - Called by the habitat to transfer habitat stat rates into the chimera's stored stats every tick.
    // - Also adds Essence to stored essence.
    public void ChimeraTick(int agility, int defense , int stamina, int strength, int wisdom)
    {
        ExperienceTick(StatType.Agility, agility);
        ExperienceTick(StatType.Defense, defense);
        ExperienceTick(StatType.Stamina, stamina);
        ExperienceTick(StatType.Strength, strength);
        ExperienceTick(StatType.Wisdom, wisdom);
        EssenceTick();
        tappable = true;

        //Debug.Log(chimeraType + " stored: " + agility + " Agility.");
        //Debug.Log(chimeraType + " stored: " + defense + " Defense.");
        //Debug.Log(chimeraType + " stored: " + stamina + " Stamina.");
        //Debug.Log(chimeraType + " stored: " + strength + " Strength.");
        //Debug.Log(chimeraType + " stored: " + wisdom + " Wisdom.");

        model.material = tappableMat;
    }

    // - Made by: Joe 2/9/2022
    // - Checks if stored experience is below cap and appropriately adds stat exp.
    private void ExperienceTick (StatType statType, int amount)
    {
        // Return if incoming is greater than cap.
        if (amount + GetStoredExpByType(statType) > experienceCap)
        {
            return;
        }

        switch (statType)
        {
            case StatType.Agility:
                storedAgilityExperience += amount;
                break;
            case StatType.Defense:
                storedDefenseExperience += amount;
                break;
            case StatType.Stamina:
                storedStaminaExperience += amount;
                break;
            case StatType.Strength:
                storedStrengthExperience += amount;
                break;
            case StatType.Wisdom:
                storedWisdomExperience += amount;
                break;
        }
    }

    // - Made by: Joe 2/9/2022
    // - Checks if stored experience is below cap and appropriately assigns.
    // - The essence formula is located here.
    private void EssenceTick()
    {
        // TODO: REVIEW THE ESSENCE FORMULA IN CLASS TO CONSIDER Happiness PROPERLY ---> DISCUSS WITH JOE AND SANTIAGO IN CLASS !!!
        // Happiness for now = x1 let´s implement it properly in class given the work done this week
        // make sure that essenceModifier is the variable intended here!!
        int essenceGain = (int)((level * 1 * baseEssenceRate) * essenceModifier);

        if(storedEssence == essenceCap)
        {
            return;
        }

        if (storedEssence + essenceGain > essenceCap)
        {
            storedEssence = essenceCap;
            Debug.Log("Cannot store anymore Essence.");
            return;
        }

        storedEssence += essenceGain;

        //Debug.Log(chimeraType + "gained: " + essenceGain + " Essence.");
    }

    // - Made by: Joe 2/2/2022
    // - On tap call HarvestEssence() and AllocateExperience() functions to appropritely gain resources that have been stored.
    // - Any other on tap interaction will go in here.
    public void ChimeraTap()
    {
        if(tappable)
        {
            HarvestEssence();
            AllocateExperience();

            tappable = false;

            Debug.Log("Tap on " + chimeraType);
            model.material = standardMat;
        }
    }

    // - Made by: Joe 2/9/2022
    // - This function is called by ChimeraTap(). On tap it will add stored essenece to the wallet.
    // - Also clears the current essence being stored.
    private void HarvestEssence()
    {
        //The GameManager will only display this value to the player therefore it is ok casting it to int here.
        GameManager.Instance.IncreaseEssence(storedEssence);
        storedEssence = 0;
    }

    // - Made by: Joao && Joe 2/9/2022
    // - Transfer experience stored by the chimera and see if each stat's threshold is met.
    // - If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        agilityExperience += storedAgilityExperience;
        if (agilityExperience >= agilityThreshold)
        {
            agilityExperience -= agilityThreshold;
            LevelUp(StatType.Agility);
        }

        defenseExperience += storedDefenseExperience;
        if (defenseExperience >= defenceThreshold)
        {
            defenseExperience -= defenceThreshold;
            LevelUp(StatType.Defense);
        }

        staminaExperience += storedStaminaExperience;
        if (staminaExperience >= staminaThreshold)
        {
            staminaExperience -= staminaThreshold;
            LevelUp(StatType.Stamina);
        }

        strengthExperience += storedStrengthExperience;
        if (strengthExperience >= strengthThreshold)
        {
            strengthExperience -= strengthThreshold;
            LevelUp(StatType.Strength);
        }

        wisdomExperience += storedWisdomExperience;
        if (wisdomExperience >= wisdomThreshold)
        {
            wisdomExperience -= wisdomThreshold;
            LevelUp(StatType.Wisdom);
        }

        // Cleanup
        storedAgilityExperience = 0;
        storedDefenseExperience = 0;
        storedStaminaExperience = 0;
        storedStrengthExperience = 0;
        storedWisdomExperience = 0;
    }

    // - Made by: Joao 2/9/2022
    // - Increase stat at rate of the relevant statgrowth variable.
    // - For example: agility += agilityGrowth, defense += defenseGrowth...
    private void LevelUp(StatType statType)
    {
        switch (statType)
        {
            case StatType.Agility:
                agility += agilityGrowth;
                Debug.Log("New " + statType + " stat = " + agility);
                break;
            case StatType.Defense:
                defense += defenseGrowth;
                Debug.Log("New " + statType + " stat = " + defense);
                break;
            case StatType.Stamina:
                stamina += staminaGrowth;
                Debug.Log("New " + statType + " stat = " + stamina);
                break;
            case StatType.Strength:
                strength += strengthGrowth;
                Debug.Log("New " + statType + " stat = " + strength);
                break;
            case StatType.Wisdom:
                wisdom += wisdomGrowth;
                Debug.Log("New " + statType + " stat = " + wisdom);
                break;
         }

        CheckEvolution();
    }

    // - Check if all requirements are ok to evolve
    private void CheckEvolution()
    {
        foreach (Chimera evolution in evolutionPaths)
        {
            if (agility < evolution.GetRequiredStats()[0])
            {
                return;
            }
            if (defense < evolution.GetRequiredStats()[1])
            {
                return;
            }
            if (stamina < evolution.GetRequiredStats()[2])
            {
                return;
            }
            if (strength < evolution.GetRequiredStats()[3])
            {
                return;
            }
            if (wisdom < evolution.GetRequiredStats()[4])
            {
                return;
            }

            Evolve(evolution);
        }
    }

    // - Evolve Chimera to its new form
    private void Evolve(Chimera newForm)
    {

    }

    #region Getters & Setters
    // Get the required stats needed to evolve
    public int[] GetRequiredStats() { return evolutionStats; }

    //  - Made by: Joe 2/9/2022
    //  - Returns stored experience based on parameter type.
    public int GetStoredExpByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Agility:
                return storedAgilityExperience;
            case StatType.Defense:
                return storedDefenseExperience;
            case StatType.Stamina:
                return storedStaminaExperience;
            case StatType.Strength:
                return storedStrengthExperience;
            case StatType.Wisdom:
                return storedStaminaExperience;
        }

        return -1;
    }
    #endregion
}