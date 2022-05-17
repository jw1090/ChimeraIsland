using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    private Chimera _chimeraToTransfer = null;
    private List<TransferIcon> _facilityShopItems = new List<TransferIcon>();

    public void Load(Chimera chimera)
    {
        _chimeraToTransfer = chimera;
    }
}
