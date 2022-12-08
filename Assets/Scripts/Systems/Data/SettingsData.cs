using System;
using System.Collections.Generic;

[Serializable]
public class SettingsData
{
    public float masterVolume = 0.0f;
    public float musicVolume = 0.0f;
    public float sfxVolume = 0.0f;
    public float ambientVolume = 0.0f;
    public float uiSfxVolume = 0.0f;
    public float cameraSpeed = 20.0f;
    public float spinSpeed = 0.8f;

    public void SetVolume(List<float> volumes)
    {
        masterVolume = volumes[0];
        musicVolume = volumes[1];
        sfxVolume = volumes[2];
        ambientVolume = volumes[3];
        uiSfxVolume = volumes[4];
    }

    public SettingsData() { }
}