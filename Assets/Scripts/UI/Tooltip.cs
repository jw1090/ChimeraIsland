using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _explanation = null;
    public bool BeingUsed = false;
    public Transform HoveringOver = null;
    public TextMeshProUGUI Explanation { get => _explanation; }

    public void LateUpdate()
    {
        BeingUsed = false;
    }
}