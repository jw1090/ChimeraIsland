using UnityEngine;

public class DisableOnAnimFinish : StateMachineBehaviour
{
    [SerializeField] private IslandFade _islandFade = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_islandFade == null)
        {
            _islandFade = animator.gameObject.GetComponentInParent<IslandFade>();
        }

        var menuManager = _islandFade;
        menuManager.StartCrossfadeDisable(stateInfo.length);
    }
}