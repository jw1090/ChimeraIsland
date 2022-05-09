using System.Collections;
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
        GameLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        StartCoroutine("WaitForEssenceManagerInit");
    }

    private IEnumerator WaitForEssenceManagerInit()
    {
        while (_essenceManager == null) 
        {
            _essenceManager = ServiceLocator.Get<EssenceManager>();
            yield return null;
        }

        while(_essenceManager.IsInitialized == false)
        {
            yield return null;
        }

        _walletText.gameObject.SetActive(true);
        _initialized = true;
    }

    public void UpdateWallet()
    {
        if (!_initialized)
        {
            return;
        }
        _walletText.text = ServiceLocator.Get<EssenceManager>().CurrentEssence.ToString();
    }
}