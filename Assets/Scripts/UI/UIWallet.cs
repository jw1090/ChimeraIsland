using TMPro;
using UnityEngine;

public class UIWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _walletText = null;
    private EssenceManager _essenceManager = null;
    private bool _initialized = false;

    private void Awake()
    {
        _walletText.gameObject.SetActive(false);
        LevelManager.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        _essenceManager = ServiceLocator.Get<EssenceManager>();
        _walletText.gameObject.SetActive(true);

        _initialized = true;

        UpdateWallet();
    }

    public void UpdateWallet()
    {
        if (_initialized == false)
        {
            return;
        }

        _walletText.text = _essenceManager.CurrentEssence.ToString();
    }
}