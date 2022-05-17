using System.Collections;
using System;
using TMPro;
using UnityEngine;

public class UIWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _walletText = null;
    private EssenceManager _essenceManager = null;
    private MonoUtil _monoUtil = null;
    private bool _initialized = false;

    private void Awake()
    {
        _walletText.gameObject.SetActive(false);
        GameLoader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        _monoUtil = ServiceLocator.Get<MonoUtil>();
        _monoUtil.StartCoroutine(WaitForEssenceManagerInit(() =>
        {
            _walletText.gameObject.SetActive(true);
            _initialized = true;
            UpdateWallet();
        }));
    }

    private IEnumerator WaitForEssenceManagerInit(Action callback)
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

        callback?.Invoke();
    }

    public void UpdateWallet()
    {
        if (!_initialized)
        {
            return;
        }
        _walletText.text = _essenceManager.CurrentEssence.ToString();
    }
}
