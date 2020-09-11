using UnityEngine;

public class PlayerWalking : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("Permissive"))
        {
            animator.SetBool("Walk", false);
        }
    }
}
