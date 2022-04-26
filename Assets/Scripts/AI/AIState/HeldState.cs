using UnityEngine;

namespace AI.Behavior
{
    public class HeldState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _chimeraBehavior.SetIsHeld(true);
            _chimeraBehavior.gameObject.GetComponent<BoxCollider>().enabled = false;
            // Debug.Log("Enter Held");
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            _chimeraBehavior.SetIsHeld(false);
            _chimeraBehavior.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}