using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAnimFinish : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var menuManager = animator.gameObject.GetComponentInParent<IslandFade>();
        menuManager.StartCrossfadeDisable(stateInfo.length);
    }
}