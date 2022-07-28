using UnityEngine;
using UnityEngine.EventSystems;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private AudioClip _clickSFX = null;
    private HabitatManager _habitatManager = null;
    private ResourceManager _resourceManager = null;
    private SceneChanger _sceneChanger = null;
    private UIManager _uiManager = null;

    //private AudioManager _audioManager = null;

    public void Initialize(UIManager uiManager)
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
        _uiManager = uiManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var chimeraGO = _resourceManager.GetChimeraBasePrefab(_chimeraType);
        Chimera chimeraComp = chimeraGO.GetComponent<Chimera>();

        ServiceLocator.Get<AudioManager>().PlaySFX(_clickSFX);

        chimeraComp.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(chimeraComp);

        _uiManager.DisableAllSceneTypeUI();
        _sceneChanger.LoadStonePlains();
    }
}