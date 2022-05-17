using AI.Behavior;
using UnityEngine;

public class FacilitiesTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chimera") == false)
        {
            return;
        }

        Facility facility = GetComponent<Facility>();
        if (facility.IsChimeraStored() == true)
        {
            return;
        }

        Chimera chimera = other.gameObject.GetComponent<Chimera>();

        facility.PlaceChimera(chimera);

        ChimeraBehavior chimeraBehaviour = chimera.gameObject.transform.GetComponent<ChimeraBehavior>();
        chimeraBehaviour.TrainingPosition = transform.position;
        Debug.Log(chimeraBehaviour.TrainingPosition);
        chimeraBehaviour.ChangeState(chimeraBehaviour.States[StateEnum.Training]);
    }
}
