using UnityEngine;

public class TempleBuyChimeras : MonoBehaviour
{
    [SerializeField] int _basePrice = 2;

    private HabitatManager _habitatManager = null;
    private ResourceManager _resourceManager = null;
    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private Temple _temple = null;
    private int _aPrice = 0;
    private int _bPrice = 0;
    private int _cPrice = 0;
    private int _aPurchaseAmount = 0;
    private int _bPurchaseAmount = 0;
    private int _cPurchaseAmount = 0;

    public int GetCurrentPrice(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                return _aPrice;
            case ChimeraType.B:
                return _bPrice;
            case ChimeraType.C:
                return _cPrice;
            default:
                Debug.LogError($"Chimera Type {chimeraType} is invalid!");
                return int.MaxValue;
        }
    }

    public void Initialize(Temple temple)
    {
        _temple = temple;

        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _aPrice = SetupPrice(ChimeraType.A);
        _bPrice = SetupPrice(ChimeraType.B);
        _cPrice = SetupPrice(ChimeraType.C);
    }

    private int SetupPrice(ChimeraType chimeraType)
    {
        int loopAmount = 0;

        switch (chimeraType)
        {
            case ChimeraType.A:
                loopAmount = _aPurchaseAmount;
                break;
            case ChimeraType.B:
                loopAmount = _bPurchaseAmount;
                break;
            case ChimeraType.C:
                loopAmount = _cPurchaseAmount;
                break;
            default:
                Debug.LogError($"Chimera Type {chimeraType} is invalid!");
                break;
        }

        int newPrice = _basePrice;

        for (int i = 0; i < loopAmount; ++i)
        {
            newPrice = PriceFormula(newPrice);
        }

        return newPrice;
    }

    private int PriceFormula(int currentPrice)
    {
        return (int)((currentPrice + 2) * 1.3f);
    }

    public void BuyChimera(EvolutionLogic evolutionLogic)
    {
        IncreasePurchaseAmount(evolutionLogic.ChimeraType);
        _uiManager.HabitatUI.DetailsManager.IncreaseChimeraSlots();

        var chimeraGO = _resourceManager.GetChimeraBasePrefab(evolutionLogic.ChimeraType);
        Chimera chimeraComp = chimeraGO.GetComponent<Chimera>();
        chimeraComp.SetIsFirstChimera(true);

        _habitatManager.AddNewChimera(chimeraComp);
        _habitatManager.ChimeraCollections.CollectChimera(evolutionLogic.ChimeraType);
        _temple.TempleCollections.Build();

        _uiManager.AlertText.CreateAlert($"You Have Acquired {evolutionLogic.Name}!");

        _audioManager.PlayUISFX(SFXUIType.PurchaseClick);
    }

    private void IncreasePurchaseAmount(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                _aPrice = PriceFormula(_aPrice);
                ++_aPurchaseAmount;
                break;
            case ChimeraType.B:
                _bPrice = PriceFormula(_bPrice);
                ++_bPurchaseAmount;
                break;
            case ChimeraType.C:
                _cPrice = PriceFormula(_cPrice);
                ++_cPurchaseAmount;
                break;
            default:
                Debug.LogError($"Chimera Type {chimeraType} is invalid!");
                break;
        }
    }
}