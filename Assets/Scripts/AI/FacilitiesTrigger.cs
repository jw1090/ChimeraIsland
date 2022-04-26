using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI.Behavior;
public class FacilitiesTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chimera")
        {
            Chimera chimera = other.gameObject.GetComponent<Chimera>();
            if (!GetComponent<Facility>().IsChimeraStored())
            {
                if (GetComponent<Facility>().IsActive() == false)
                {
                    return;
                }
                GetComponent<Facility>().PlaceChimera(chimera);
                ChimeraBehavior chimeraBehaviour = chimera.gameObject.transform.GetComponent<ChimeraBehavior>();
                chimeraBehaviour.SetTrainingPos(this.gameObject.transform.position);
                Debug.Log(chimeraBehaviour.GetTrainingPos());
                chimeraBehaviour.ChangeState(chimeraBehaviour.states[StateEnum.Training]);
            }
        }
    }
}
