using System.Collections;
using UnityEngine;

public class GlowMarker : MonoBehaviour
{
    private Transform _trainingPos = null;
    private Facility _facility = null;

    public void ActivateGlow(bool activate)
    {
        if (activate == false)
        {
            if (this.gameObject.activeInHierarchy == true)
            {
                StartCoroutine(GlowReset());
            }
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void Initialize(Facility facility)
    {
        _facility = facility;

        _trainingPos = this.transform;

        this.gameObject.SetActive(false);
    }

    private IEnumerator GlowReset()
    {
        yield return new WaitForSeconds(0.02f);
        this.gameObject.SetActive(false);
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

        _facility.PlaceChimeraFromUI(chimera);

        chimera.SetInFacility(true);

        chimeraBehaviour.ChangeState(ChimeraBehaviorState.Training);
        chimeraBehaviour.gameObject.transform.position = _trainingPos.position;
    }
}