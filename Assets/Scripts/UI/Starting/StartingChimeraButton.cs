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

    //private void ChimeraName()
    //{
    //    switch (_chimeraType)
    //    {
    //        case ChimeraType.None:
    //            break;
    //        case ChimeraType.A:
    //            _nameText.text = "Nauphant";
    //            break;
    //        case ChimeraType.B:
    //            _nameText.text = "Frolli";
    //            break;
    //        case ChimeraType.C:
    //            _nameText.text = "Patchero";
    //            break;
    //        default:
    //            break;
    //    }
    //}
}