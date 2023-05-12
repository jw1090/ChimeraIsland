using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawn : MonoBehaviour
{
    [SerializeField] private StatefulObject _crystal = null;
    [SerializeField] private List<ParticleSystem> _tapMine = new List<ParticleSystem>();
    [SerializeField] private List<Crystal> _crystalList = new List<Crystal>();
    [SerializeField] private List<Outline> _outlines = new List<Outline>();
    [SerializeField] private CrystalFloatingText _floatingText = null;
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

        _floatingText.Initialize();

        Outline(false);
    }

    public void Activate(int currentTier)
    {
        foreach (Crystal crystal in _crystalList)
        {
            crystal.ResetCrystal();
        }

        _crystal.SetState($"Crystal{currentTier}");

        _floatingText.Activate();

        _currentTier = currentTier;
        _health = _currentTier;

        _isActive = true;
        _crystal.gameObject.SetActive(true);
        _crystal.CurrentState.StateObject.GetComponent<Crystal>().Grow(currentTier);
    }

    public void Harvest()
    {
        if (_isActive == false)
        {
            return;
        }

        ShowEffect();

        int cost = 25 * _currentTier;

        _currencyManager.IncreaseEssence(cost);
        _floatingText.Click(cost);

        --_health;

        if (_health == 2)
        {
            _crystal.SetState($"Crystal{_health}");
            _audioManager.PlaySFX(EnvironmentSFXType.MiningTap);
        }
        else if (_health == 1)
        {
            _crystal.SetState($"Crystal{_health}");
            _audioManager.PlaySFX(EnvironmentSFXType.MiningTap);
        }
        else if (_health == 0)
        {
            _isActive = false;
            _crystal.gameObject.SetActive(false);

            _audioManager.PlaySFX(EnvironmentSFXType.MiningHarvest);
        }
    }

    private void ShowEffect()
    {
        foreach (ParticleSystem p in _tapMine)
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

    public void Outline(bool glow)
    {
        foreach (Outline outline in _outlines)
        {
            outline.enabled = glow;
        }
    }
}