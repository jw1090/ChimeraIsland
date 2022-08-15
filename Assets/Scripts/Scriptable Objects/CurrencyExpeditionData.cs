using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyExpeditionData", menuName = "ScriptableObjects/CurrencyExpedition", order = 1)]
public class CurrencyExpeditionData : ExpeditionBaseData
{
    public float AmountGained = 0.0f;

    public CurrencyExpeditionData ShallowCopy()
    {
        return (CurrencyExpeditionData)MemberwiseClone();
    }
}