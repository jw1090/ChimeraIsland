using UnityEngine;

public class StartingUI : MonoBehaviour
{
    [SerializeField] private StartingChimeraButton _optionAChimeraButton;
    [SerializeField] private StartingChimeraButton _optionBChimeraButton;
    [SerializeField] private StartingChimeraButton _optionCChimeraButton;

    public void Initialize(UIManager uiManager)
    {
        _optionAChimeraButton.Initialize(uiManager);
        _optionBChimeraButton.Initialize(uiManager);
        _optionCChimeraButton.Initialize(uiManager);
    }
}