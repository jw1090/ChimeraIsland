using AI.Behavior;
using UnityEngine;

public class FacilitiesTrigger : MonoBehaviour
{
    private BoxCollider _boxCollider = null;
    private Facility _facility = null;

    private void Awake()
    {
        LevelManager.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _facility = GetComponent<Facility>();
        _boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chimera") == false)
        {
            return;
        }

        if (_facility.IsChimeraStored() == true)
        {
            return;
        }

        Chimera chimera = other.transform.gameObject.GetComponent<EvolutionLogic>().ChimeraBrain;
        ChimeraBehavior chimeraBehaviour = chimera.GetComponent<ChimeraBehavior>();

        if (chimeraBehaviour.Dropped == false)
        {
            return;
        }

        _facility.PlaceChimera(chimera);

        chimeraBehaviour.TrainingPosition = transform.position;
        chimeraBehaviour.ChangeState(chimeraBehaviour.States[StateEnum.Training]);
    }
}