using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    [SerializeField] private List<TransferButton> _transferButtons = new List<TransferButton>();
    private Chimera _chimeraToTransfer = null;

    public Chimera ChimeraToTransfer { get => _chimeraToTransfer; }

    public void ResetChimera() { _chimeraToTransfer = null; }

    public void Initialize()
    {
        foreach(TransferButton transferButton in _transferButtons)
        {
            transferButton.Initialize(this);
        }
    }

    public void Open(Chimera chimera)
    {
        _chimeraToTransfer = chimera;
        gameObject.SetActive(true);
    }
}