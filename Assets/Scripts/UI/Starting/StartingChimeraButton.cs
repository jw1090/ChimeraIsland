using UnityEngine;
using UnityEngine.EventSystems;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    private HabitatManager _habitatManager = null;
    private ResourceManager _resourceManager = null;
    private SceneChanger _sceneChanger = null;
    private bool _clicked = false;

    public void SetChimeraType(ChimeraType chimeraType) { _chimeraType = chimeraType; }

    public void Initialize()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
    }

    public void SetupStartingButton()
    {
        _clicked = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clicked == true)
        {
            return;
        }

        _clicked = true;

        var chimeraGO = _resourceManager.GetChimeraBasePrefab(_chimeraType);
        Chimera chimeraComp = chimeraGO.GetComponent<Chimera>();
        chimeraComp.SetIsFirstChimera(true);
        chimeraComp.SetHabitatType(HabitatType.StonePlains);

        _habitatManager.AddNewChimera(chimeraComp);

        _sceneChanger.LoadStonePlains();
    }
}