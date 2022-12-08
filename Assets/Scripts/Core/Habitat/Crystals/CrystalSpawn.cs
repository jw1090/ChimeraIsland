using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawn : MonoBehaviour
{
    [SerializeField] private StatefulObject _crystal = null;
    [SerializeField] private List<ParticleSystem> _tapMine = new List<ParticleSystem>();
    private CurrencyManager _currencyManager = null;
    private AudioManager _audioManager = null;
    private int _health = 3;
    private bool _isActive = false;
    private int _currentTier = 1;

    public bool IsActive { get => _isActive; }

    public void Initialize()
    {
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _isActive = false;
        _crystal.gameObject.SetActive(false);
    }

    public void Activate(int currentTier)
    {
        _crystal.SetState($"Crystal{currentTier}");

        _currentTier = currentTier;
        _health = _currentTier;

        _isActive = true;
        _crystal.gameObject.SetActive(true);
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
            _crystal.gameObject.SetActive(false);
            _currencyManager.IncreaseEssence(20 * _currentTier);

            _audioManager.PlaySFX(EnvironmentSFXType.MiningHarvest);
        }
        else
        {
            if (_health == 1)
            {
                _currencyManager.IncreaseEssence(15 * _currentTier);
            }
            else
            {
                _currencyManager.IncreaseEssence(10 * _currentTier);
            }

            _audioManager.PlaySFX(EnvironmentSFXType.MiningTap);
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