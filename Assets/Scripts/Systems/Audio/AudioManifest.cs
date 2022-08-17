using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AudioManifest")]
public class AudioManifest : ScriptableObject
{
    public List<AudioClipItem> AudioItems = new List<AudioClipItem>();
}
