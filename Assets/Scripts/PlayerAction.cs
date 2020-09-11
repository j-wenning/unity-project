using System.Collections;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerBase m_Base;

    private bool mCanBasicAttack = true;

    public void TryBasicAttack()
    {
        if (mCanBasicAttack
        && Input.GetButton("Fire2")
        && m_Base.m_State.IsTag("Interruptible"))
        {
            StartCoroutine(BasicAttack());
        }
    }

    private IEnumerator BasicAttack()
    {
        mCanBasicAttack = false;
        yield return new WaitForEndOfFrame();
        mCanBasicAttack = true;
    }
}
