using TMPro;
using UnityEngine;

public class UIWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _walletText = null;

    public void UpdateWallet()
    {
        _walletText.text = ServiceLocator.Get<EssenceManager>().CurrentEssence.ToString();
    }
}