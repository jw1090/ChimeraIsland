using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ElementalType elementalType = ElementalType.None;
    [SerializeField] private StatType statType = StatType.None;
    [SerializeField] private ChimeraModel currentChimeraModel = null;
    [SerializeField] private int price = 200;

    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private int levelCap = 99;
    [SerializeField] private int endurance = 0;
    [SerializeField] private int intelligence = 0;
    [SerializeField] private int strength = 0;
    [SerializeField] private int happiness = 0;
    [SerializeField] private int happinessMod = 1;

    [Header("Stat Growth")]
    [SerializeField] private int enduranceGrowth = 1;
    [SerializeField] private int intelligenceGrowth = 1;
    [SerializeField] private int strengthGrowth = 1;
    [SerializeField] private int enduranceExperience = 0;
    [SerializeField] private int intelligenceExperience = 0;
    [SerializeField] private int strengthExperience = 0;
    [SerializeField] private int enduranceThreshold = 5;
    [SerializeField] private int intelligenceThreshold = 5;
    [SerializeField] private int strengthThreshold = 5;
    [SerializeField] private int levelUpTracker = 0;

    [Header("Stored Experience")]
    [SerializeField] private int storedEnduranceExperience = 0;
    [SerializeField] private int storedIntelligenceExperience = 0;
    [SerializeField] private int storedStrengthExperience = 0;
    [SerializeField] private int experienceCap = 200;

    [Header("Essence")]
    [SerializeField] private float baseEssenceRate = 5;
    [SerializeField] private int storedEssence = 0;
    [SerializeField] private float essenceModifier = 1.0f; // Tuning knob for essence gain
    [SerializeField] private int essenceCap = 100;

    // - Made by: Joe 2/9/2022
    // - Checks if stored experience is below cap and appropriately adds stat exp.
    public void ExperienceTick (StatType statType, int amount)
    {
        // Return if incoming is greater than cap.
        if (amount + GetStoredExpByType(statType) > experienceCap)
        {
            return;
        }

        switch (statType)
        {
            case StatType.Endurance:
                storedEnduranceExperience += amount;
                break;
            case StatType.Intelligence:
                storedIntelligenceExperience += amount;
                break;
            case StatType.Strength:
                storedStrengthExperience += amount;
                break;
        }
    }

    // - Made by: Joe 2/9/2022
    // - Checks if stored experience is below cap and appropriately assigns.
    // - The essence formula is located here.
    public void EssenceTick()
    { 
        happinessMod = HappinessModifierCalc();
        //Debug.Log("Current Happiness Modifier: " + happinessMod);
        
        // Sqrt is used to gain diminishing returns on levels.
        // EssenceModifier is used to tune the level scaling
        int essenceGain = (int)((happinessMod * baseEssenceRate) + Mathf.Sqrt(level * essenceModifier));

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
        //HappinessCheck();
            HarvestEssence();
            if (level < levelCap)
            {
                AllocateExperience();
            }
    }

    
    // - Made by: Santiago 3/02/2022
    // - Happiness can range between -100 and 100.
    // - At -100, happinessMod is 0.3. At 0, it is 1. At 100 it is 3.
    private int HappinessModifierCalc()
    {
        if (happiness == 0)
        {
            return 1;
        }
        else if (happiness > 0)
        {
            int hapMod = (happiness) / 50 + 1;
            return hapMod;
        }
        else
        {
            int hapMod = (1 * (int)Mathf.Sqrt(happiness + 100) / 15) + (1 / 3);
            return hapMod;
        }
    }

    // - Made by: Joe 2/9/2022
    // - This function is called by ChimeraTap(). On tap it will add stored essenece to the wallet.
    // - Also clears the current essence being stored.
    private void HarvestEssence()
    {
        // The GameManager will only display this value to the player therefore it is ok casting it to int here.
        GameManager.Instance.IncreaseEssence(storedEssence);
        storedEssence = 0;
    }

    // - Made by: Joao && Joe 2/9/2022
    // - Transfer experience stored by the chimera and see if each stat's threshold is met.
    // - If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        bool levelUp = false;

        enduranceExperience += storedEnduranceExperience;
        if (enduranceExperience >= enduranceThreshold)
        {
            enduranceExperience -= enduranceThreshold;
            levelUp = true;
            LevelUp(StatType.Endurance);

            enduranceThreshold += (int)(Mathf.Sqrt(enduranceThreshold) * 1.2f);
        }

        intelligenceExperience += storedIntelligenceExperience;
        if (intelligenceExperience >= intelligenceThreshold)
        {
            intelligenceExperience -= intelligenceThreshold;
            levelUp = true;
            LevelUp(StatType.Intelligence);

            intelligenceThreshold += (int)(Mathf.Sqrt(intelligenceThreshold) * 1.2f);
        }

        strengthExperience += storedStrengthExperience;
        if (strengthExperience >= strengthThreshold)
        {
            strengthExperience -= strengthThreshold;
            levelUp = true;
            LevelUp(StatType.Strength);

            strengthThreshold += (int)(Mathf.Sqrt(strengthThreshold) * 1.2f);
        }

        if (levelUp)
        {
            currentChimeraModel.CheckEvolution(intelligence, endurance, strength);
        }

        // Cleanup
        storedEnduranceExperience = 0;
        storedIntelligenceExperience = 0;
        storedStrengthExperience = 0;
    }

    // - Made by: Joao 2/9/2022
    // - Increase stat at rate of the relevant statgrowth variable.
    // - For example: agility += agilityGrowth, defense += defenseGrowth...
    private void LevelUp(StatType statType)
    {
        switch (statType)
        {
            case StatType.Endurance:
                endurance += enduranceGrowth;
                Debug.Log("New " + statType + " stat = " + endurance);
                break;
            case StatType.Intelligence:
                intelligence += intelligenceGrowth;
                Debug.Log("New " + statType + " stat = " + intelligence);
                break;
            case StatType.Strength:
                strength += strengthGrowth;
                Debug.Log("New " + statType + " stat = " + strength);
                break;
         }

        ++levelUpTracker;

        if(levelUpTracker % 5 == 0)
        {
            ++level;
            Debug.Log("LEVEL UP! " + this.gameObject + " is now level " + level + " !");
        }
    }


    //public int HappinessCheck(ElementalType compareType)
    //{
    //    if (Mathf.Abs(compareType - elementalType) == 3)
    //    {
    //        return 1;
    //    }
    //    else if (Mathf.Abs(compareType - elementalType) == 1)
    //    {
    //        return -1;
    //    }

    //    if (compareType == ElementalType.Fira || elementalType == ElementalType.Fira && compareType == ElementalType.Aero || elementalType == ElementalType.Aero)
    //    {
    //        return -1;
    //    }
    //    return 0;
    //}

    //private void HappinessCheck()
    //{
    //    if (GameManager.Instance.ElementalAffinityCheck(GetElementalType()) == 1)
    //    {
    //        IncreaseHappiness(1);
    //    }
    //    if (GameManager.Instance.ElementalAffinityCheck(GetElementalType()) == -1)
    //    {
    //        IncreaseHappiness(-1);
    //    }
    //}



    //public void HappinessCheck()
    //{
      
    //    //else if (Mathf.Abs(compareType - elementalType) == 1)
    //    //{
    //    //    return -1;
    //    //}

    //    //if (compareType == ElementalType.Fira || elementalType == ElementalType.Fira && compareType == ElementalType.Aero || elementalType == ElementalType.Aero)
    //    //{
    //    //    return -1;
    //    //}
    //    //return 0;
    //}

    #region Getters & Setters
    public int GetStoredExpByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Endurance:
                return storedEnduranceExperience;
            case StatType.Strength:
                return storedStrengthExperience;
            case StatType.Intelligence:
                return storedIntelligenceExperience;
        }

        return -1;
    }
    public int GetStatByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Endurance:
                return endurance;
            case StatType.Intelligence:
                return intelligence;
            case StatType.Strength:
                return strength;
            case StatType.Happiness:
                return happiness;
        }

        return -1;
    }
    public int GetLevel() { return level; }
    public int GetPrice() { return price; }
    public ElementalType GetElementalType() { return elementalType; }
    public StatType GetStatType() { return statType; }
    public Texture2D GetIcon() { return currentChimeraModel.GetIcon(); }
    public void SetModel(ChimeraModel model) { currentChimeraModel = model; }
    public void IncreaseHappiness(int amount)
    {
        happiness += amount;
        if(happiness > 100)
        {
            happiness = 100;
        }

        if(happiness < -100)
        {
            happiness = -100;
        }
    }
    #endregion
}