using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_Rigidbody;

    [SerializeField, Tooltip("Player speed (scaled by delta time)")]
    private float m_MoveSpeed = 800f;
    [SerializeField, Tooltip("Minimum required distance to move player (prevents jittering)")]
    private float m_MoveThreshold = 0.2f;

    private Vector2 mNewPos;
    private Vector2 mPrevPos;
    private Vector2 mPosDiff;
    private Vector2 mNewNormal;
    private Vector2 mVelocity;
    private float mScaledMoveSpeed;

    private void Start()
    {
        mNewPos = transform.position;
    }

    private void FixedUpdate()
    {
        mScaledMoveSpeed = m_MoveSpeed * Time.deltaTime;
        SetNewPos(
            Input.GetMouseButton(1)
            ? (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
            : mNewPos
        );
    }

    private void SetNewPos(Vector2 pos)
    {
        mNewPos = pos;
        mPrevPos = transform.localPosition;
        mPosDiff.Set(mNewPos.x - mPrevPos.x, mNewPos.y - mPrevPos.y);
        mNewNormal.Set(mPosDiff.normalized.x, mPosDiff.normalized.y);
        mVelocity = m_Rigidbody.velocity;
        if (mPosDiff.magnitude <= m_MoveThreshold) mVelocity.Set(0, 0);
        else mVelocity.Set(mNewNormal.x * mScaledMoveSpeed, mNewNormal.y * mScaledMoveSpeed);
        m_Rigidbody.velocity = mVelocity;
    }
}
