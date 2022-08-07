using UnityEngine;
using UnityEngine.EventSystems;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    private HabitatManager _habitatManager = null;
    private ResourceManager _resourceManager = null;
    private SceneChanger _sceneChanger = null;
    private AudioManager _audioManager = null;
    private UIManager _uiManager = null;

    public void Initialize(UIManager uiManager)
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _uiManager = uiManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var chimeraGO = _resourceManager.GetChimeraBasePrefab(_chimeraType);
        Chimera chimeraComp = chimeraGO.GetComponent<Chimera>();

        //_audioManager.PlayElementsSFX(ElementsSFX.ConfirmClick);

        chimeraComp.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(chimeraComp);

        _uiManager.DisableAllSceneTypeUI();
        _sceneChanger.LoadStonePlains();
    }
}