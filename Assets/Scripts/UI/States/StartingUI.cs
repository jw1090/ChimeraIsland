using UnityEngine;

public class StartingUI : MonoBehaviour
{
    [SerializeField] private StartingChimeraButton _optionAChimeraButton = null;
    [SerializeField] private StartingChimeraButton _optionBChimeraButton = null;
    [SerializeField] private StartingChimeraButton _optionCChimeraButton = null;
    [SerializeField] private StartingChimeraInfo _startingChimeraInfo = null;

    public StartingChimeraButton StartingA { get => _optionAChimeraButton; }
    public StartingChimeraButton StartingB { get => _optionBChimeraButton; }
    public StartingChimeraButton StartingC { get => _optionCChimeraButton; }

    public void SetAudioManager(AudioManager audioManager)
    {
        _optionAChimeraButton.SetAudioManager(audioManager);
        _optionBChimeraButton.SetAudioManager(audioManager);
        _optionCChimeraButton.SetAudioManager(audioManager);
    }

    public void Initialize()
    {
        _optionAChimeraButton.Initialize();
        _optionBChimeraButton.Initialize();
        _optionCChimeraButton.Initialize();
    }

    public void SetupStartingButtons()
    {
        _optionAChimeraButton.SetupStartingButton();
        _optionBChimeraButton.SetupStartingButton();
        _optionCChimeraButton.SetupStartingButton();
    }
}