using AI.Behavior;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ElementalType _elementalType = ElementalType.None;
    [SerializeField] private StatType _statPreference = StatType.None;
    [SerializeField] private Passives _passive = Passives.None;
    [SerializeField] private int _price = 50;
    [SerializeField] private bool _inFacility = false;

    [Header("Stats")]
    [SerializeField] private int _levelCap = 99;
    [SerializeField] private int _happinessMod = 1;

    [Header("Stat Growth")]
    [SerializeField] private int _enduranceGrowth = 1;
    [SerializeField] private int _intelligenceGrowth = 1;
    [SerializeField] private int _strengthGrowth = 1;
    [SerializeField] private int _enduranceExperience = 0;
    [SerializeField] private int _intelligenceExperience = 0;
    [SerializeField] private int _strengthExperience = 0;
    [SerializeField] private int _enduranceThreshold = 5;
    [SerializeField] private int _intelligenceThreshold = 5;
    [SerializeField] private int _strengthThreshold = 5;
    [SerializeField] private int _levelUpTracker = 0;

    [Header("Essence")]
    [SerializeField] private float _baseEssenceRate = 5;
    [SerializeField] private float _essenceModifier = 1.0f; // Tuning knob for essence gain

    [Header("References")]
    [SerializeField] private EvolutionLogic _currentEvolution = null;
    [SerializeField] private EssenceManager _essenceManager = null;

    public int Level { get; set; } = 1;
    public int Endurance { get; set; } = 0;
    public int Intelligence { get; set; } = 0;
    public int Strength { get; set; } =  0;
    public int Happiness { get; set; } = 0;

    public ElementalType GetElementalType() { return _elementalType; }
    public StatType GetStatPreference() { return _statPreference; }
    public Passives GetPassive() { return _passive; }
    public int GetPrice() { return _price; }
	public ChimeraType GetChimeraType() { return _currentEvolution.GetChimeraType(); }
    public void SetChimeraType(ChimeraType type) { _currentEvolution.SetChimeraType(type); }
    public Sprite GetIcon() { return _currentEvolution.GetIcon(); }
	public bool GetStatByType(StatType statType, out int amount)
    {
        amount = 0;
        switch (statType)
        {
            case StatType.Endurance:
                amount = Endurance;
                return true;
            case StatType.Intelligence:
                amount = Intelligence;
                return true;
            case StatType.Strength:
                amount = Strength;
                return true;
            case StatType.Happiness:
                amount = Happiness;
                return true;
            default:
                Debug.LogError("Default StatType please change!");
                break;
        }
        return false;
    }
    public void ChangeHappiness(int amount)
    {
        if (Happiness + amount >= 100)
        {
            Happiness = 100;
            return;
        }
        else if (Happiness + amount <= -100)
        {
            Happiness = -100;
            return;
        }

        Happiness += amount;
    }
    public void SetModel(EvolutionLogic evolution) { _currentEvolution = evolution; }
    public void SetInFacility(bool inFacility) { _inFacility = inFacility; }

    public void CreateChimera(Habitat habitat, EssenceManager essenceManager)
    {
        Debug.Log("<color=Green> Creating Chimera: " + this + " </color>");
        _essenceManager = essenceManager;
        InitializeEvolution();
        GetComponent<ChimeraBehavior>().Initialize(habitat);
    }

    private void InitializeEvolution()
    {
        _currentEvolution = GetComponentInChildren<EvolutionLogic>();
        _currentEvolution.SetChimeraBrain(this);
        _currentEvolution.SetChimeraType(GetChimeraType());
        _currentEvolution.LoadEvolution();
    }

    // Checks if stored experience is below cap and appropriately adds stat exp.
    public void ExperienceTick (StatType statType, int amount)
    {
        if(Level >= _levelCap)
        {
            return;
        }

        switch (statType)
        {
            case StatType.Endurance:
                _enduranceExperience += amount;
                break;
            case StatType.Intelligence:
                _intelligenceExperience += amount;
                break;
            case StatType.Strength:
                _strengthExperience += amount;
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
        _happinessMod = HappinessModifierCalc();
        // Debug.Log("Current Happiness Modifier: " + happinessMod);

        if (_inFacility)
        {
            if (_passive == Passives.Multitasking)
            {
               MultitaskingTick();
            }
            return;
        }

        // Sqrt is used to gain diminishing returns on levels.
        // EssenceModifier is used to tune the level scaling
        int essenceGain = (int)((_happinessMod * _baseEssenceRate) + Mathf.Sqrt(Level * _essenceModifier));
        _essenceManager.IncreaseEssence(essenceGain);
    }
    private void MultitaskingTick()
    {
        int essenceGain = (int)((_happinessMod * _baseEssenceRate) + Mathf.Sqrt(Level * _essenceModifier) * 0.5f);
        _essenceManager.IncreaseEssence(essenceGain);
    }
    public void HappinessTick()
    {
        if (!_inFacility)
        {
            int happinessAmount = -1;

            if(_passive == Passives.GreenThumb)
            {
                List<Chimera> chimeras = ServiceLocator.Get<Habitat>().Chimeras;

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
            ServiceLocator.Get<UIManager>().UpdateDetails();
        }
    }

    // Happiness can range between -100 and 100.
    // At -100, happinessMod is 0.3. At 0, it is 1. At 100 it is 3.
    private int HappinessModifierCalc()
    {
        if (Happiness == 0)
        {
            return 1;
        }
        else if (Happiness > 0)
        {
            int hapMod = (Happiness) / 50 + 1;
            return hapMod;
        }
        else
        {
            int hapMod = (1 * (int)Mathf.Sqrt(Happiness + 100) / 15) + (1 / 3);
            return hapMod;
        }
    }

    // Transfer experience stored by the chimera and see if each stat's threshold is met.
    // If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        bool levelUp = false;

        if (_enduranceExperience >= _enduranceThreshold)
        {
            _enduranceExperience -= _enduranceThreshold;
            levelUp = true;
            LevelUp(StatType.Endurance);

            _enduranceThreshold += (int)(Mathf.Sqrt(_enduranceThreshold) * 1.2f);
        }

        if (_intelligenceExperience >= _intelligenceThreshold)
        {
            _intelligenceExperience -= _intelligenceThreshold;
            levelUp = true;
            LevelUp(StatType.Intelligence);

            _intelligenceThreshold += (int)(Mathf.Sqrt(_intelligenceThreshold) * 1.2f);
        }

        if (_strengthExperience >= _strengthThreshold)
        {
            _strengthExperience -= _strengthThreshold;
            levelUp = true;
            LevelUp(StatType.Strength);

            _strengthThreshold += (int)(Mathf.Sqrt(_strengthThreshold) * 1.2f);
        }

        if (levelUp)
        {
            _currentEvolution.CheckEvolution(Endurance, Intelligence, Strength);
        }
    }

    // Increase stat at rate of the relevant statgrowth variable.
    private void LevelUp(StatType statType)
    {
        switch (statType)
        {
            case StatType.Endurance:
                Endurance += _enduranceGrowth;
                Debug.Log(this + " gained " + statType + " stat = " + Endurance);
                break;
            case StatType.Intelligence:
                Intelligence += _intelligenceGrowth;
                Debug.Log(this + " gained " + statType + " stat = " + Intelligence);
                break;
            case StatType.Strength:
                Strength += _strengthGrowth;
                Debug.Log(this + " gained " + statType + " stat = " + Strength);
                break;
            default:
                Debug.LogError("Default Level Up Please Change!");
                break;
        }

        ServiceLocator.Get<UIManager>().UpdateDetails();
        ++_levelUpTracker;

        if (_levelUpTracker % 3 == 0)
        {
            ++Level;
            Debug.Log("LEVEL UP! " + gameObject + " is now level " + Level + " !");
        }
    }
}
