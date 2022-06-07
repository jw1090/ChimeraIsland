using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartingChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ChimeraType _chimeraType = ChimeraType.None;
    HabitatManager _habitatManager = null;
    ChimeraCreator _chimeraCreator = null;

    public void Initialize()
    {
        _chimeraCreator = ServiceLocator.Get<ToolsManager>().ChimeraCreator;
        _habitatManager = ServiceLocator.Get<HabitatManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject newChimera = _chimeraCreator.CreateChimeraByType(_chimeraType);
        Chimera chimera = newChimera.GetComponent<Chimera>();

        chimera.SetHabitatType(HabitatType.StonePlains);
        _habitatManager.AddNewChimera(newChimera.GetComponent<Chimera>());

        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES_SCENE);
    }
}