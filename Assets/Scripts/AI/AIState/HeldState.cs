using UnityEngine;

namespace AI.Behavior
{
    public class HeldState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _chimeraBehavior.gameObject.GetComponent<BoxCollider>().enabled = false;
            if (_chimeraBehavior.animator != null)
                _chimeraBehavior.animator.SetBool("Walk", false);
            Debug.Log("Enter Held");
        }

        public override void Update()
        {
            _chimeraBehavior.ObjFollowMouse();
            _chimeraBehavior.HeldEnterPatrol();
        }

        public override void Exit()
        {
            if (_chimeraBehavior.animator != null)
                _chimeraBehavior.animator.SetBool("Walk", true);
            _chimeraBehavior.HeldExit();
            _chimeraBehavior.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}