using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType chimeraType = ChimeraType.None;
    [SerializeField] private ElementalType elementalType = ElementalType.None;
    [SerializeField] private Texture2D profileIcon = null;

    [Header("Egg Info")]
    [SerializeField] private bool isEgg = false;
    [SerializeField] private int clicksToHatch = 3;
    [SerializeField] private int price = 500;

    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private int levelCap = 99;
    [SerializeField] private int stamina = 0;
    [SerializeField] private int strength = 0;
    [SerializeField] private int wisdom = 0;
    [SerializeField] private int happiness = 0;
    [SerializeField] private int happinessMod = 1;

    [Header("Stat Growth")]
    [SerializeField] private int staminaGrowth = 1;
    [SerializeField] private int strengthGrowth = 1;
    [SerializeField] private int wisdomGrowth = 1;
    [SerializeField] private int staminaExperience = 0;
    [SerializeField] private int strengthExperience = 0;
    [SerializeField] private int wisdomExperience = 0;
    [SerializeField] private int staminaThreshold = 5;
    [SerializeField] private int strengthThreshold = 5;
    [SerializeField] private int wisdomThreshold = 5;
    [SerializeField] private int levelUpTracker = 0;

    [Header("Stored Experience")]
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


    // - Made by: Joe 2/9/2022
    // - Called by the habitat to transfer habitat stat rates into the chimera's stored stats every tick.
    // - Also adds Essence to stored essence.
    private void Start()
    {
        if(isEgg && experienceCap != 0)
        {
            experienceCap = 0;
            Debug.Log( "ALERT: " + this.gameObject + " is an Egg. The experienceCap has been zeroed");
        }
    }

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

            if (isEgg && clicksToHatch > 0)
            {
                --clicksToHatch;
                Debug.Log("Remaining Clicks to Hatch: " + clicksToHatch);
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
        staminaExperience += storedStaminaExperience;
        if (staminaExperience >= staminaThreshold)
        {
            staminaExperience -= staminaThreshold;
            LevelUp(StatType.Stamina);

            staminaThreshold += (int)(Mathf.Sqrt(staminaThreshold) * 1.2f);
        }

        strengthExperience += storedStrengthExperience;
        if (strengthExperience >= strengthThreshold)
        {
            strengthExperience -= strengthThreshold;
            LevelUp(StatType.Strength);

            strengthThreshold += (int)(Mathf.Sqrt(strengthThreshold) * 1.2f);
        }

        wisdomExperience += storedWisdomExperience;
        if (wisdomExperience >= wisdomThreshold)
        {
            wisdomExperience -= wisdomThreshold;
            LevelUp(StatType.Wisdom);

            wisdomThreshold += (int)(Mathf.Sqrt(wisdomThreshold) * 1.2f);
        }

        CheckEvolution();

        // Cleanup
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

        ++levelUpTracker;

        if(levelUpTracker % 5 == 0)
        {
            ++level;
            Debug.Log("LEVEL UP! " + this.gameObject + " is now level " + level + " !");
        }
    }

    // - Check if all requirements are ok to evolve
    private void CheckEvolution()
    {
        if (evolutionPaths.Length == 0)
        {
            return;
        }

        foreach (Chimera evolution in evolutionPaths)
        {
            // If it is an Egg evolve regardless
            if (isEgg)
            {
                if(clicksToHatch == 0)
                {
                    Evolve(evolution);
                }
                return;
            }
            // If it is NOT an Egg, evaluate stats before evolve
            if (stamina < evolution.GetRequiredStats()[0])
            {
                continue;
            }
            if (strength < evolution.GetRequiredStats()[1])
            {
                continue;
            }
            if (wisdom < evolution.GetRequiredStats()[2])
            {
                continue;
            }

            Evolve(evolution);
            return;
        }
    }

    // - Made by: Joe 4/5/2022
    // - Evolve Chimera to its new form
    private void Evolve(Chimera newForm)
    {
        // Instantiate new chimera
        Chimera child = this;
        Chimera evolution = Instantiate(newForm, transform.position, Quaternion.identity, transform.parent);

        evolution.SetEvolutionStats
            (
                level, stamina, strength, wisdom, levelUpTracker,
                staminaThreshold, strengthThreshold, wisdomThreshold,
                happiness, happinessMod
            );

        Debug.Log("Spawned:" + evolution);
        transform.parent.parent.GetComponent<Habitat>().EvolveSwap(ref child, ref evolution);

        // Destroy old
        Destroy(this.gameObject);
    }

    /*
    public int HappinessCheck(ElementalType compareType)
    {
        if(Mathf.Abs(compareType - elementalType) == 3)
        {   
            return 1;
        }
        else if (Mathf.Abs(compareType - elementalType) == 1)
        {
            return -1;
        }

        if(compareType == ElementalType.Fira || elementalType == ElementalType.Fira && compareType == ElementalType.Aero || elementalType == ElementalType.Aero)
        {
            return -1;
        }
        return 0;
    }
    */
    /*
    private void HappinessCheck()
    {
        if(GameManager.Instance.ElementalAffinityCheck(GetElementalType()) == 1)
        {
            happiness++;
            if(happiness > 100)
            {
                happiness = 100;
            }
        }
        if (GameManager.Instance.ElementalAffinityCheck(GetElementalType()) == -1)
        {
            happiness--;
            if(happiness < -100)
            {
                happiness = -100;
            }
        }
    }
    */

    #region Getters & Setters
    // Get the required stats needed to evolve
    public int[] GetRequiredStats() { return evolutionStats;}
    public Texture2D getProfileIcon() { return profileIcon; }
    public int GetStoredExpByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Stamina:
                return storedStaminaExperience;
            case StatType.Strength:
                return storedStrengthExperience;
            case StatType.Wisdom:
                return storedStaminaExperience;
        }

        return -1;
    }
    public int GetStatByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Stamina:
                return stamina;
            case StatType.Strength:
                return strength;
            case StatType.Wisdom:
                return wisdom;
            case StatType.Happiness:
                return happiness;
        }

        return -1;
    }
    public int GetLevel() { return level; }
    public void SetEvolutionStats
        (
            int newLevel, int newStamina, int newStrength, int newWisdom, int newLevelUpTracker,
            int newStaminaThreshold, int newStrengthThreshold, int newWisdomThreshold,
            int newHappiness, int newHappinessMod
        )
    {
        level = newLevel;
        stamina = newStamina;
        strength = newStrength;
        wisdom = newWisdom;
        levelUpTracker = newLevelUpTracker;

        staminaThreshold = newStaminaThreshold;
        strengthThreshold = newStrengthThreshold;
        wisdomThreshold = newWisdomThreshold;

        happiness = newHappiness;
        happinessMod = newHappinessMod;
    }

    public int GetPrice() { return price; }

    public ElementalType GetElementalType() { return elementalType; }

    public Texture2D GetProfileIcon() { return profileIcon; }

    #endregion
}