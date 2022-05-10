using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    private List<TransferIcon> _transferIcons = new List<TransferIcon>();

    public void Initialize(Habitat habitat)
    {
        foreach (Transform child in transform)
        {
            TransferIcon transferIcon = child.GetComponent<TransferIcon>();

            _transferIcons.Add(transferIcon);
            transferIcon.Initialize(habitat);
        }
    }
}
