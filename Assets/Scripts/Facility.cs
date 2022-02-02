using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType facilityType = FacilityType.None;
    [SerializeField] private StatType statType = StatType.None;
    [SerializeField] private int currentTier = 1;
    [SerializeField] private int statAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpgradeFacility()
    {

    }
    public void Remove()
    {

    }

    public int [] GetStats()
    {
        int[] stats = { };
        return stats;
    }
    public int GetTier()
    {
        return 0;
    }
}
