using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class StartingChimeraButton : MonoBehaviour
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    private HabitatManager _habitatManager = null;
    private ResourceManager _resourceManager = null;
    private SceneChanger _sceneChanger = null;
    private bool _clicked = false;

    public Button Button { get => GetComponent<Button>(); }
    public ChimeraType ChimeraType { get => _chimeraType; }
    public void SetChimeraType(ChimeraType chimeraType) { _chimeraType = chimeraType; }

    public void Initialize()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();


        //ChimeraName();
    }

    public void SetupStartingButton()
    {
        _clicked = false;
    }

    public void ChimeraClicked(ChimeraType chimeraType)
    {
        var chimeraGO = _resourceManager.GetChimeraBasePrefab(_chimeraType);
        Chimera chimeraComp = chimeraGO.GetComponent<Chimera>();
        chimeraComp.SetIsFirstChimera(true);
        chimeraComp.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(chimeraComp);

        _sceneChanger.LoadStonePlains();
    }

    public string GetChimeraName()
    {
        switch (_chimeraType)
        {
            case ChimeraType.None:
                return null;
            case ChimeraType.A:
                return "Nauphant";
            case ChimeraType.B:
                return "Frolli";
            case ChimeraType.C:
                return "Patchero";
            default:
                return null;
        }
    }
    public string GetChimeraBio()
    {
        switch (_chimeraType)
        {
            case ChimeraType.None:
                return null;
            case ChimeraType.A:
                return "He's blue and likes water, like Squirtle but as an mini elephant.";
            case ChimeraType.B:
                return "Goofy ahh creature";
            case ChimeraType.C:
                return "Santiago's Favourite, this is HIM, this is THE chimera.";
            default:
                return null;
        }
    }

    public ElementType GetChimeraElement()
    {
        switch (_chimeraType)
        {
            case ChimeraType.None:
                return ElementType.None;
            case ChimeraType.A:
                return ElementType.Aqua;
            case ChimeraType.B:
                return ElementType.Fira;
            case ChimeraType.C:
                return ElementType.Bio;
            default:
                return ElementType.None;
        }
    }
}