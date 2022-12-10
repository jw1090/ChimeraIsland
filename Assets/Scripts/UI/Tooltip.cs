using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _explanation = null;

    public TextMeshProUGUI Explanation { get => _explanation; }

    public bool BeingUsed { get; set; }
    public Transform HoveringOver { get; set; }

    public void LateUpdate()
    {
        BeingUsed = false;
    }
}