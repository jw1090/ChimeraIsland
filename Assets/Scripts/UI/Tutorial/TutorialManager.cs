using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Dictionary<TextAsset, List<Chimera>> _tutorialShow = new Dictionary<TextAsset, List<Chimera>>();

    private UIManager _uiManager = null;
    private UITutorialOverlay _tutorialOverlay = null;

    private void Awake()
    {
        //LevelManager.CallOnComplete(Initialize);
    }

    public TutorialManager Initialize()
    {
        _uiManager = ServiceLocator.Get<UIManager>();
        return this;
    }

    public void ShowTutorial()
    {
        _tutorialOverlay = _uiManager.GetTutorialOveraly();
        _tutorialOverlay.ShowOverlay();
    }
}
