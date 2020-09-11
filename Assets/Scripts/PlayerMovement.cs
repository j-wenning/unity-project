using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerBase m_Base;

    [SerializeField, Tooltip("Player speed (scaled by delta time)")]
    private float m_MoveSpeed = 800f;
    [SerializeField, Tooltip("Minimum required distance to move player (prevents jittering)")]
    private float m_MoveThreshold = 0.2f;
    [SerializeField]
    private float m_DashSpeed;
    [SerializeField]
    private float m_DashDuration;
    [SerializeField, Tooltip("Dash cooldown")]
    private float m_DashCD;

    private Vector2 mNewPos;
    private Vector2 mPrevPos;
    private Vector2 mPosDiff;
    private Vector2 mDashVector;
    private bool mIsMoving;
    private bool mCanDash = true;
    private float mDashEndTime;

    private void Awake()
    {
        mNewPos = transform.position;
    }

    public void TryMoveToPos()
    {
        if (!m_Base.m_State.IsTag("Interruptible")) return;
        if (Input.GetMouseButton(1)) mNewPos = m_Base.m_MousePos;
        mPrevPos = transform.localPosition;
        mPosDiff = mNewPos - mPrevPos;
        mIsMoving = mPosDiff.magnitude > m_MoveThreshold;
        m_Base.m_Rigidbody.velocity = mIsMoving
            ? mPosDiff.normalized * m_MoveSpeed * m_Base.m_DeltaTime
            : Vector2.zero;
        m_Base.m_Animator.SetBool("Walk", mIsMoving);
    }
    public void TryDash()
    {
        if (mCanDash
        && Input.GetButton("Fire1")
        && m_Base.m_State.IsTag("Interruptible"))
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        mCanDash = false;
        m_Base.m_Animator.SetBool("Dash", true);
        mDashVector = (m_Base.m_MousePos - (Vector2)transform.position).normalized * m_DashSpeed * m_Base.m_DeltaTime;
        mDashEndTime = m_Base.m_FixedTime + m_DashDuration;
        do
        {
            m_Base.m_Rigidbody.velocity = mDashVector;
            yield return new WaitForFixedUpdate();
        }
        while (m_Base.m_FixedTime < mDashEndTime);
        m_Base.m_Animator.SetBool("Dash", false);
        m_Base.m_Rigidbody.velocity = Vector2.zero;
        mNewPos = transform.position;
        TryMoveToPos();
        yield return new WaitForSeconds(m_DashCD);
        mCanDash = true;
    }
}
