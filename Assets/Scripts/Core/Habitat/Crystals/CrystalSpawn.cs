using UnityEngine;

public class CrystalSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _crystal = null;
    private CurrencyManager _currencyManager = null;
    private AudioManager _audioManager = null;
    private Habitat _habitat = null;
    private int _health = 3;
    private bool _isActive = false;

    public bool IsActive { get => _isActive; }

    public void Initialize()
    {
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

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
}