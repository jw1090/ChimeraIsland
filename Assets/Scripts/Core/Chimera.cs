using UnityEngine;
using UnityEngine.UI;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ElementalType elementalType = ElementalType.None;
    [SerializeField] private StatType statPreference = StatType.None;
    [SerializeField] private int price = 200;
    [SerializeField] private bool inFacility = false;

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

    [Header("Essence")]
    [SerializeField] private float baseEssenceRate = 5;
    [SerializeField] private float essenceModifier = 1.0f; // Tuning knob for essence gain

    [Header("References")]
    [SerializeField] private ChimeraModel currentChimeraModel = null;

    // - Made by: Joe 2/9/2022
    // - Checks if stored experience is below cap and appropriately adds stat exp.
    public void ExperienceTick (StatType statType, int amount)
    {
        if(level >= levelCap)
        {
            return;
        }

        switch (statType)
        {
            case StatType.Endurance:
                enduranceExperience += amount;
                break;
            case StatType.Intelligence:
                intelligenceExperience += amount;
                break;
            case StatType.Strength:
                strengthExperience += amount;
                break;
        }

        AllocateExperience();
    }

    // - Made by: Joe 2/9/2022
    // - Checks if stored experience is below cap and appropriately assigns.
    // - The essence formula is located here.
    public void EssenceTick()
    { 
        if(inFacility)
        {
            return;
        }

        happinessMod = HappinessModifierCalc();
        //Debug.Log("Current Happiness Modifier: " + happinessMod);
        
        // Sqrt is used to gain diminishing returns on levels.
        // EssenceModifier is used to tune the level scaling
        int essenceGain = (int)((happinessMod * baseEssenceRate) + Mathf.Sqrt(level * essenceModifier));
        GameManager.Instance.IncreaseEssence(essenceGain);

        // Debug.Log(chimeraType + "gained: " + essenceGain + " Essence.");
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

    // - Made by: Joao && Joe 2/9/2022
    // - Transfer experience stored by the chimera and see if each stat's threshold is met.
    // - If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        bool levelUp = false;

        if (enduranceExperience >= enduranceThreshold)
        {
            enduranceExperience -= enduranceThreshold;
            levelUp = true;
            LevelUp(StatType.Endurance);

            enduranceThreshold += (int)(Mathf.Sqrt(enduranceThreshold) * 1.2f);
        }

        if (intelligenceExperience >= intelligenceThreshold)
        {
            intelligenceExperience -= intelligenceThreshold;
            levelUp = true;
            LevelUp(StatType.Intelligence);

            intelligenceThreshold += (int)(Mathf.Sqrt(intelligenceThreshold) * 1.2f);
        }

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

        GameManager.Instance.UpdateDetailsUI();
        ++levelUpTracker;

        if (levelUpTracker % 3 == 0)
        {
            ++level;
            Debug.Log("LEVEL UP! " + gameObject + " is now level " + level + " !");
        }
    }

    #region Getters & Setters
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
    public StatType GetStatPreference() { return statPreference; }
    public Sprite GetIcon() { return currentChimeraModel.GetIcon(); }
    public void SetModel(ChimeraModel model) { currentChimeraModel = model; }
    public void SetInFacility(bool facilityState) { inFacility = facilityState; }
    public void IncreaseHappiness(int amount)
    {
        if(happiness + amount >= 100)
        {
            happiness = 100;
            return;
        }
        else if(happiness + amount <= -100)
        {
            happiness = -100;
            return;
        }

        happiness += amount;
    }
    #endregion
}