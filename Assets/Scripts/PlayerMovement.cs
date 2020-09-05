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

    private void Awake()
    {
        mNewPos = transform.position;
    }

    private void FixedUpdate()
    {
        SetNewPos();
    }

    private void SetNewPos()
    {
        if (Input.GetMouseButton(1)) mNewPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPrevPos = transform.localPosition;
        mPosDiff = mNewPos - mPrevPos;
        m_Rigidbody.velocity = mPosDiff.magnitude > m_MoveThreshold
            ? mPosDiff.normalized * m_MoveSpeed * Time.deltaTime
            : Vector2.zero;
    }
}
