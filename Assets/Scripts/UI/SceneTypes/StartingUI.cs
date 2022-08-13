using UnityEngine;

public class StartingUI : MonoBehaviour
{
    [SerializeField] private StartingChimeraButton _optionAChimeraButton;
    [SerializeField] private StartingChimeraButton _optionBChimeraButton;
    [SerializeField] private StartingChimeraButton _optionCChimeraButton;

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
}