using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : MonoBehaviour
{

    //Create new list to save the active facilities position
    public List<Transform> transformsFacilities;

    public Transform[] directPoints;


    // add position after active
    public void AddFacilitiesPos(Transform add)
    {
        transformsFacilities.Add(add);
    }

}
