using UnityEngine;
using UnityEngine.EventSystems;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    private HabitatManager _habitatManager = null;
    private PersistentData _persistentData = null;
    private ResourceManager _resourceManager = null;
    private SceneChanger _sceneChanger = null;
    private TutorialManager _tutorialManager = null;
    private UIManager _uiManager = null;

    public void Initialize(UIManager uiManager)
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _uiManager = uiManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var chimeraGO = _resourceManager.GetChimeraBasePrefab(_chimeraType);
        Chimera chimeraComp = chimeraGO.GetComponent<Chimera>();

        chimeraComp.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(chimeraComp);

        _tutorialManager.ResetTutorialProgress();

        _uiManager.DisableAllSceneTypeUI();
        _sceneChanger.LoadStonePlains();
    }
}