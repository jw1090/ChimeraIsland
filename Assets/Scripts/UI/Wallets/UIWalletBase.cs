using TMPro;
using UnityEngine;

public abstract class UIWalletBase : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _walletText = null;
    protected CurrencyManager _currencyManager = null;
    protected bool _initialized = false;

    public void Initialize()
    {
        _currencyManager = ServiceLocator.Get<CurrencyManager>();

        _initialized = true;
        UpdateWallet();

        _walletText.gameObject.SetActive(true);
    }

    public abstract void UpdateWallet();
}