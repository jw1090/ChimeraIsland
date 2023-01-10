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

    public void ActivateEffect(TapVFXType type, RaycastHit hit)
    {
        switch (type)
        {
            case TapVFXType.Ground:
                foreach( ParticleSystem p in _tapGround)
                {
                    if (p.isPlaying != true)
                    {
                        StartCoroutine(StartEffect(p, hit, 1.1f));
                        break;
                    }
                }
                _audioManager.PlaySFX(EnvironmentSFXType.DirtHit);
                break;
            case TapVFXType.Water:
                foreach (ParticleSystem p in _tapWater)
                {
                    if (p.isPlaying != true)
                    {
                        StartCoroutine(StartEffect(p, hit, .6f));
                        break;
                    }
                }
                _audioManager.PlaySFX(EnvironmentSFXType.WaterHit);
                break;
            case TapVFXType.Stone:
                foreach (ParticleSystem p in _tapStone)
                {
                    if (p.isPlaying != true)
                    {
                        StartCoroutine(StartEffect(p, hit, 1.1f));
                        break;
                    }
                }
                _audioManager.PlaySFX(EnvironmentSFXType.StoneHit);
                break;
            case TapVFXType.Tree:
                _audioManager.PlaySFX(EnvironmentSFXType.TreeHit);
                break;
            default:
                Debug.LogError($"Unhandled TapVFXType [{type}] please fix!");
                break;
        }
    }

    private IEnumerator StartEffect(ParticleSystem effect, RaycastHit hit, float effectLength)
    {
        effect.transform.rotation = Quaternion.FromToRotation(hit.collider.transform.up, hit.normal) * hit.collider.transform.rotation;
        effect.transform.Rotate(new Vector3(-90, 90, 0));
        effect.gameObject.SetActive(true);
        effect.time = 0;
        effect.gameObject.transform.position = hit.point;
        yield return new WaitForSeconds(effectLength);
        effect.Stop();
    }
}
