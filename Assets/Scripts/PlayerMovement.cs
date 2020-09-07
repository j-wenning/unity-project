using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_Rigidbody;
    [SerializeField]
    private Animator m_Animator;

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
    private bool mCanDash;
    private float mDashEndTime;

    private void Awake()
    {
        mNewPos = transform.position;
        mCanDash = true;
    }

    private void FixedUpdate()
    {
        MoveToPos();
        StartDash();
    }

    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void MoveToPos()
    {
        if (!(m_Animator.GetBool("Walking") || m_Animator.GetBool("Idling"))) return;
        if (Input.GetMouseButton(1)) mNewPos = GetMousePos();
        mPrevPos = transform.localPosition;
        mPosDiff = mNewPos - mPrevPos;
        mIsMoving = mPosDiff.magnitude > m_MoveThreshold;
        m_Rigidbody.velocity = mIsMoving
            ? mPosDiff.normalized * m_MoveSpeed * Time.deltaTime
            : Vector2.zero;
        m_Animator.SetBool("Walking", mIsMoving);
    }
    private void StartDash()
    {
        if (mCanDash && Input.GetButton("Fire1")) StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        mCanDash = false;
        m_Animator.SetBool("Dashing", true);
        mDashVector = (GetMousePos() - (Vector2)transform.position).normalized * m_DashSpeed * Time.deltaTime;
        mDashEndTime = Time.fixedTime + m_DashDuration;
        do
        {
            m_Rigidbody.velocity = mDashVector;
            yield return new WaitForFixedUpdate();
        } while (Time.fixedTime < mDashEndTime);
        m_Animator.SetBool("Dashing", false);
        m_Rigidbody.velocity = Vector2.zero;
        mNewPos = transform.position;
        MoveToPos();
        yield return new WaitForSeconds(m_DashCD);
        mCanDash = true;
    }
}
