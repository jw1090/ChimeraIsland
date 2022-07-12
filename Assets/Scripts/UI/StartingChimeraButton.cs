using UnityEngine;
using UnityEngine.EventSystems;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ChimeraType _chimeraType = ChimeraType.None;
    HabitatManager _habitatManager = null;
    ResourceManager _resourceManager = null;
    SceneChanger _sceneChanger = null;

    public void Initialize()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var chimeraGO = _resourceManager.GetChimeraBasePrefab(_chimeraType);
        Chimera chimeraComp = chimeraGO.GetComponent<Chimera>();

        chimeraComp.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(chimeraComp);

        ServiceLocator.Get<TutorialManager>().ResetTutorialProgress();
        ServiceLocator.Get<UIManager>().DisableHabitatUI();
        _sceneChanger.LoadStonePlains();
    }
}