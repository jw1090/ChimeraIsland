using System.Collections.Generic;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ElementalType elementalType = ElementalType.None;
    [SerializeField] private StatType statPreference = StatType.None;
    [SerializeField] private Passives passive = Passives.None;
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

    // Checks if stored experience is below cap and appropriately adds stat exp.
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
            default:
                Debug.LogError("Default Experience Tick Please Change!");
                break;
        }

        AllocateExperience();
    }

    // Checks if stored experience is below cap and appropriately assigns.
    // The essence formula is located here.
    public void EssenceTick()
    {
        if (inFacility)
        {
            if (passive == Passives.Multitasking)
            {
               MultitaskingTick();
            }
            return;
        }

        happinessMod = HappinessModifierCalc();
        // Debug.Log("Current Happiness Modifier: " + happinessMod);
        
        // Sqrt is used to gain diminishing returns on levels.
        // EssenceModifier is used to tune the level scaling
        int essenceGain = (int)((happinessMod * baseEssenceRate) + Mathf.Sqrt(level * essenceModifier));
        GameManager.Instance.IncreaseEssence(essenceGain);

        // Debug.Log(chimeraType + "gained: " + essenceGain + " Essence.");
    }
    private void MultitaskingTick()
    {
        happinessMod = HappinessModifierCalc();
        int essenceGain = ((int)((happinessMod * baseEssenceRate) + Mathf.Sqrt(level * essenceModifier))/2);
        GameManager.Instance.IncreaseEssence(essenceGain);
    }
    public void HappinessTick()
    {
        if (!inFacility)
        {
            int happinessAmount = -1;

            if(passive == Passives.GreenThumb)
            {
                List<Chimera> chimeras = GameManager.Instance.GetActiveHabitat().GetChimeras();

                foreach (Chimera chimera in chimeras)
                {
                    if(chimera.GetElementalType() == ElementalType.Bio)
                    {
                        happinessAmount = 1;
                        ChangeHappiness(happinessAmount);
                    }
                }
            }
            ChangeHappiness(happinessAmount);
            GameManager.Instance.UpdateDetailsUI();
        }
    }

    // Happiness can range between -100 and 100.
    // At -100, happinessMod is 0.3. At 0, it is 1. At 100 it is 3.
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

    // Transfer experience stored by the chimera and see if each stat's threshold is met.
    // If so, LevelUp is called with specific stat enumerator.
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
            currentChimeraModel.CheckEvolution(endurance, intelligence, strength);
        }
    }

    // Increase stat at rate of the relevant statgrowth variable.
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
            default:
                Debug.LogError("Default Level Up Please Change!");
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
            default:
                Debug.LogError("Default StatType please change!");
                break;
        }
        return -1;
    }
    public int GetLevel() { return level; }
    public int GetPrice() { return price; }
    public ElementalType GetElementalType() { return elementalType; }
    public StatType GetStatPreference() { return statPreference; }
    public Passives GetPassive() { return passive; }
    public Sprite GetIcon() { return currentChimeraModel.GetIcon(); }
    public void SetModel(ChimeraModel model) { currentChimeraModel = model; }
    public void SetInFacility(bool facilityState) { inFacility = facilityState; }
    public void ChangeHappiness(int amount)
    {
        if (happiness + amount >= 100)
        {
            happiness = 100;
            return;
        }
        else if (happiness + amount <= -100)
        {
            happiness = -100;
            return;
        }

        happiness += amount;
    }
    #endregion
}