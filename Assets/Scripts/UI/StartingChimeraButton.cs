using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    private HabitatManager _habitatManager = null;
    private ResourceManager _resourceManager = null;
    private SceneChanger _sceneChanger = null;
    private AudioManager _audioManager = null;
    private bool _clicked = false;

    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }

    public void Initialize()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
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

        chimeraComp.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(chimeraComp);

        _audioManager.PlayUISFX(SFXUIType.ConfirmClick);

        _sceneChanger.LoadStonePlains();
    }
}