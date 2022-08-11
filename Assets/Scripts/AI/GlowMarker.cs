using AI.Behavior;
using UnityEngine;

public class GlowMarker : MonoBehaviour
{
    private Transform _trainingPos = null;
    private BoxCollider _boxCollider = null;
    private MeshRenderer _meshRenderer = null;
    private Facility _facility = null;

    public void ActivateGlowCollider(bool activate) { _boxCollider.enabled = activate; }
    public void ActivateGlowRenderer(bool activate) { _meshRenderer.enabled = activate; }

    public void Initialize(Facility facility)
    {
        _facility = facility;

        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _trainingPos = this.transform;

        ActivateGlowCollider(false);
        ActivateGlowRenderer(false);
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

        chimera.SetInFacility(true);

        chimeraBehaviour.ChangeState(chimeraBehaviour.States[StateEnum.Training]);
        chimeraBehaviour.gameObject.transform.position = _trainingPos.position;
    }
}