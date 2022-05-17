using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferIcon : MonoBehaviour
{
    [Header("Transfer Info")]
    [SerializeField] private HabitatType _habitatType = HabitatType.None;

    [Header("References")]
    [SerializeField] private TransferButton _transferButton = null;

    public void Initialize(Habitat habitat)
    {
        _transferButton.Initialize(habitat, _habitatType);
    }
}
