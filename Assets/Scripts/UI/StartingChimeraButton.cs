using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private TextMeshProUGUI _nameText = null;
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

        ChimeraName();
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

        chimeraComp.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(chimeraComp);

        _audioManager.PlayUISFX(SFXUIType.ConfirmClick);

        _sceneChanger.LoadStonePlains();
    }

    private void ChimeraName()
    {
        switch (_chimeraType)
        {
            case ChimeraType.None:
                break;
            case ChimeraType.A:
                _nameText.text = "Nauphant";
                break;
            case ChimeraType.B:
                _nameText.text = "Frolli";
                break;
            case ChimeraType.C:
                _nameText.text = "Patchero";
                break;
            default:
                break;
        }
    }
}