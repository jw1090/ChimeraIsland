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
        if (_tapMine[0].isPlaying != true)
        {
            _tapMine[0].gameObject.SetActive(true);
            _tapMine[0].time = 0;
            StartCoroutine(StopMineEffect(_tapMine[0]));
        }
        else if (_tapMine[1].isPlaying != true)
        {
            _tapMine[1].gameObject.SetActive(true);
            _tapMine[1].time = 0;
            StartCoroutine(StopMineEffect(_tapMine[1]));
        }
        else if (_tapMine[2].isPlaying != true)
        {
            _tapMine[2].gameObject.SetActive(true);
            _tapMine[2].time = 0;
            StartCoroutine(StopMineEffect(_tapMine[2]));
        }
    }

    IEnumerator StopMineEffect(ParticleSystem _mine)
    {
        yield return new WaitForSeconds(.8f);
        _mine.Stop();
    }
}