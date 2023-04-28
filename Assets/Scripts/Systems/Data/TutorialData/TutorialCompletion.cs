using System;

[Serializable]
public class TutorialCompletionData
{
    public bool _intro = false;
    public bool _expeditionSelection = false;
    public bool _expeditionSetup = false;
    public bool _facilities = false;
    public bool _reccomendedTraits = false;
    public bool _fossilShop = false;
    public bool _facilityUpgrades = false;
    public bool _failure = false;
    public bool _butChimera = false;
    public bool collections = false;
    public bool _upgrade = false;

    public bool IsCompleted(TutorialStageType type)
    {
        switch (type)
        {
            case TutorialStageType.Intro:
                return _intro;
            case TutorialStageType.ExpeditionSelection:
                return _expeditionSelection;
            case TutorialStageType.ExpeditionSetup:
                return _expeditionSetup;
            case TutorialStageType.Facilities:
                return _facilities;
            case TutorialStageType.ReccomendedTraits:
                return _reccomendedTraits;
            case TutorialStageType.Temple:
                return _fossilShop;
            case TutorialStageType.FacilityUpgrades:
                return _facilityUpgrades;
            case TutorialStageType.Failure:
                return _failure;
            case TutorialStageType.BuyChimera:
                return _butChimera;
            case TutorialStageType.Collections:
                return collections;
            case TutorialStageType.Upgrade:
                return _upgrade;
            default:
                return true;
        }
    }

    public void Complete(TutorialStageType type)
    {
        switch (type)
        {
            case TutorialStageType.Intro:
                _intro = true;
                break;
            case TutorialStageType.ExpeditionSelection:
                _expeditionSelection = true;
                break;
            case TutorialStageType.ExpeditionSetup:
                _expeditionSetup= true;
                break;
            case TutorialStageType.Facilities:
                _facilities= true;
                break;
            case TutorialStageType.ReccomendedTraits:
                _reccomendedTraits= true;
                break;
            case TutorialStageType.Temple:
                _fossilShop= true;
                break;
            case TutorialStageType.FacilityUpgrades:
                _facilityUpgrades= true;
                break;
            case TutorialStageType.Failure:
                _failure = true;
                break;
            case TutorialStageType.BuyChimera:
                _butChimera = true;
                break;
            case TutorialStageType.Collections:
                collections = true;
                break;
            case TutorialStageType.Upgrade:
                _upgrade = true;
                break;
            default:
                return;
        }
    }

    public void Reset()
    {
        _intro = false;
        _expeditionSelection = false;
        _expeditionSetup = false;
        _facilities = false;
        _reccomendedTraits = false;
        _fossilShop = false;
        _facilityUpgrades = false;
        _failure = false;
        _butChimera = false;
        collections = false;
        _upgrade = false;
    }
}
