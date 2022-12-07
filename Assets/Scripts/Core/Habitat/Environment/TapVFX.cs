using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapVFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _tapGround = new List<ParticleSystem>();
    [SerializeField] private List<ParticleSystem> _tapWater = new List<ParticleSystem>();
    [SerializeField] private List<ParticleSystem> _tapStone = new List<ParticleSystem>();
    private AudioManager _audioManager = null;

    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }

    public void ActivateEffect(TapVFXType type, Vector3 position)
    {
        switch (type)
        {
            case TapVFXType.ground:
                foreach( ParticleSystem p in _tapGround)
                {
                    if (p.isPlaying != true)
                    {
                        StartCoroutine(StartEffect(p, position, 1.1f));
                        break;
                    }
                }
                _audioManager.PlayUISFX(SFXUIType.DirtHit);
                break;
            case TapVFXType.water:
                foreach (ParticleSystem p in _tapWater)
                {
                    if (p.isPlaying != true)
                    {
                        StartCoroutine(StartEffect(p, position, .6f));
                        break;
                    }
                }
                _audioManager.PlayUISFX(SFXUIType.WaterHit);
                break;
            case TapVFXType.stone:
                foreach (ParticleSystem p in _tapStone)
                {
                    if (p.isPlaying != true)
                    {
                        StartCoroutine(StartEffect(p, position, 1.1f));
                        break;
                    }
                }
                _audioManager.PlayUISFX(SFXUIType.StoneHit);
                break;
            default:
                Debug.LogError($"Unhandled TapVFXType [{type}] please fix!");
                break;
        }
    }

    private IEnumerator StartEffect(ParticleSystem effect, Vector3 position, float effectLength)
    {
        effect.gameObject.SetActive(true);
        effect.time = 0;
        effect.gameObject.transform.position = position;
        yield return new WaitForSeconds(effectLength);
        effect.Stop();
    }
}
