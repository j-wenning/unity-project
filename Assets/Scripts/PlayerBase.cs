using System.Configuration;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public PlayerMovement m_Movement;
    public PlayerAction m_Action;
    public Rigidbody2D m_Rigidbody;
    public Collider2D m_Collider;
    public Animator m_Animator;

    [HideInInspector]
    public AnimatorStateInfo m_State;
    [HideInInspector]
    public float m_DeltaTime;
    [HideInInspector]
    public float m_FixedTime;
    [HideInInspector]
    public Vector2 m_MousePos;

    private void FixedUpdate()
    {
        m_FixedTime = Time.fixedTime;
        m_DeltaTime = Time.fixedDeltaTime;
        m_State = m_Animator.GetCurrentAnimatorStateInfo(m_Animator.GetLayerIndex("Base"));
        m_MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_Movement.TryMoveToPos();
        m_Movement.TryDash();
        m_Action.TryBasicAttack();
    }
}
