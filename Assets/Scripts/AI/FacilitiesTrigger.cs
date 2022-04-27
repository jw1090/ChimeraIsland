using AI.Behavior;
using UnityEngine;

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
                chimeraBehaviour.TrainingPosition = transform.position;
                Debug.Log(chimeraBehaviour.TrainingPosition);
                chimeraBehaviour.ChangeState(chimeraBehaviour.States[StateEnum.Training]);
            }
        }
    }
}
