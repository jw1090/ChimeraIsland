using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITraining : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private StatType _type;
    private Chimera _chimera;
    int cost;
    int amount;
    public void intializeChimera(Chimera chimera)
    {
        _chimera = chimera;

    }
    public void OnClickDecrease()
    {

    }
    public void OnClickIncrease()
    {

    }
    public void OnClickExit()
    {

    }
    public void OnClickConfirm()
    {

    }

}
