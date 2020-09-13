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
    public Vector2 m_LocalPos;
    [HideInInspector]
    public Vector2 m_MousePos;
    [HideInInspector]
    public float m_DeltaTime;
    [HideInInspector]
    public float m_FixedTime;

    private void Awake()
    {
        CacheVals();
        m_Animator.Update(m_DeltaTime);
    }

    private void FixedUpdate()
    {
        CacheVals();
        m_Movement.GetMoveInput();
        m_Movement.Move();
        m_Action.TryBasicAttack();
    }

    private void CacheVals()
    {
        m_FixedTime = Time.fixedTime;
        m_DeltaTime = Time.fixedDeltaTime;
        m_LocalPos = transform.localPosition;
        m_MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_State = m_Animator.GetCurrentAnimatorStateInfo(m_Animator.GetLayerIndex("Base"));
    }
}
