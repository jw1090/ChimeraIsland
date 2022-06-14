using TMPro;
using UnityEngine;

public class UIWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _walletText = null;
    private EssenceManager _essenceManager = null;
    private bool _initialized = false;

    public void Initialize()
    {
        _essenceManager = ServiceLocator.Get<EssenceManager>();

        _initialized = true;
        UpdateWallet();

        _walletText.gameObject.SetActive(true);
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