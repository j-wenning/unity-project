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

    private void Start()
    {
        mNewPos = transform.position;
    }

    private void FixedUpdate()
    {
        SetNewPos(
            Input.GetMouseButton(1)
            ? (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
            : mNewPos
        );
    }

    private void SetNewPos(Vector2 pos)
    {
        mNewPos = pos;
        mPrevPos = transform.position;
        mPosDiff = (mNewPos - mPrevPos);
        mNewNormal = mPosDiff.normalized;
        m_Rigidbody.velocity = mPosDiff.magnitude > m_MoveThreshold
            ? mNewNormal * m_MoveSpeed * Time.deltaTime
            : Vector2.zero;
    }
}
