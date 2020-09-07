using UnityEngine;

public class PlayerWalking : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Dashing")) animator.SetBool("Walking", false);
    }
}
