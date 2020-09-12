using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerBase m_Base;

    [SerializeField]
    private float m_MoveSpeed;
    [SerializeField, Tooltip("Minimum distance for a player to select to move (prevents jittering)")]
    private float m_MoveThreshold;
    [SerializeField, Range(0, 180), Tooltip("Maximum automatic change in trajectory before stopping the player")]
    private int m_StopAngle;
    [SerializeField, Tooltip("Minimum speed to not be considered \"stuck\"")]
    private float m_StuckTolerance;
    [SerializeField, Tooltip("Number of frames at \"Stuck Tolerance\" required to be determined as \"stuck\"")]
    private int m_StuckFrames;
    [SerializeField]
    private float m_DashSpeed;
    [SerializeField]
    private float m_DashLength;
    [SerializeField]
    private float m_DashCooldown;

    private Vector2 mNewPos;
    private Vector2 mPosDiff;
    private Vector2 mOldPosDiff;
    private int mStuckCount = 0;
    private bool mIsMoving = false;
    private bool mIsDashing = false;
    private bool mCanDash = true;

    private void OnEnable()
    {
        mNewPos = m_Base.m_LocalPos;
    }

    public void Move()
    {
        mPosDiff = mNewPos - m_Base.m_LocalPos;
        mIsMoving = Vector2.Angle(mPosDiff, mOldPosDiff) < m_StopAngle
                    && mPosDiff.magnitude > Mathf.Epsilon;
        CheckStuck();
        mIsDashing = mIsDashing && mIsMoving;
        mOldPosDiff = mPosDiff;
        m_Base.m_Rigidbody.velocity = mPosDiff.normalized *
                    (mIsMoving ? (mIsDashing ? m_DashSpeed : m_MoveSpeed) : 0);
        if (!mIsMoving) mNewPos = m_Base.m_LocalPos;
        m_Base.m_Animator.SetBool("Walk", mIsMoving && !mIsDashing);
        m_Base.m_Animator.SetBool("Dash", mIsDashing);
    }

    private void CheckStuck()
    {
        if (m_Base.m_Rigidbody.velocity.magnitude > m_StuckTolerance)
        {
            mStuckCount = 0;
        }
        else if (++mStuckCount > m_StuckFrames)
        {
            mStuckCount = 0;
            mIsMoving = false;
        }
    }

    public void GetMoveInput()
    {
        if (m_Base.m_State.IsTag("Interruptible"))
        {
            if (mCanDash && Input.GetButton("Fire1"))
            {
                mNewPos = m_Base.m_LocalPos + (m_Base.m_MousePos - m_Base.m_LocalPos).normalized * m_DashLength;
                mIsDashing = true;
                mCanDash = false;
                mPosDiff = mOldPosDiff = Vector2.zero;
                StartCoroutine(ApplyDashCooldown());
            }
            else if (Input.GetMouseButton(1))
            {
                if ((m_Base.m_MousePos - m_Base.m_LocalPos).magnitude > m_MoveThreshold)
                {
                    mNewPos = m_Base.m_MousePos;
                }
            }
        }
    }

    private IEnumerator ApplyDashCooldown()
    {
        yield return new WaitForSeconds(m_DashCooldown);
        mCanDash = true;
    }
}
