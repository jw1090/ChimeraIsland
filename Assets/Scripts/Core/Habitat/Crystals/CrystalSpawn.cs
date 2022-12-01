using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _crystal = null;
    [SerializeField] private List<ParticleSystem> _tapMine = new List<ParticleSystem>();
    private CurrencyManager _currencyManager = null;
    private AudioManager _audioManager = null;
    private Habitat _habitat = null;
    private int _health = 3;
    private bool _isActive = false;

    public bool IsActive { get => _isActive; }

    public void Initialize(Habitat habitat)
    {
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _habitat = habitat;

        _isActive = false;
        _crystal.SetActive(false);
    }

    public void Activate()
    {
        _health = 3;
        _isActive = true;
        _crystal.SetActive(true);
    }

    public void Harvest()
    {
        if (_isActive == false)
        {
            return;
        }
        ShowEffect();
        if (--_health == 0)
        {
            _isActive = false;
            _crystal.SetActive(false);
            _currencyManager.IncreaseEssence(20 * _habitat.CurrentTier);

            _audioManager.PlayUISFX(SFXUIType.Harvest);
        }
        else
        {
            if (_health == 1)
            {
                _currencyManager.IncreaseEssence(15 * _habitat.CurrentTier);
            }
            else
            {
                _currencyManager.IncreaseEssence(10 * _habitat.CurrentTier);
            }

            _audioManager.PlayUISFX(SFXUIType.Hit);
        }
    }

    private void ShowEffect()
    {
        foreach(ParticleSystem p in _tapMine)
        {
            if (p.isPlaying != true)
            {
                p.gameObject.SetActive(true);
                p.time = 0;
                StartCoroutine(StopMineEffect(p));
                break;
            }
        }
    }

    IEnumerator StopMineEffect(ParticleSystem _mine)
    {
        yield return new WaitForSeconds(.8f);
        _mine.Stop();
    }
}