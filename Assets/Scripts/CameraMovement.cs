using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Player;

    [SerializeField]
    private float m_Distance = 0.05f;
    [SerializeField]
    private float m_Time = 0.1f;
    [SerializeField, Tooltip("Distance from screen edge to activate camera movement")]
    private int m_BorderWidth = 25;

    private Vector3 mNewPos;

    private void Start()
    {
        mNewPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        mNewPos.y += Input.mousePosition.y >= Screen.height - m_BorderWidth
            ? m_Distance
            : Input.mousePosition.y <= m_BorderWidth
            ? -m_Distance
            : 0;
        mNewPos.x += Input.mousePosition.x >= Screen.width - m_BorderWidth
            ? m_Distance
            : Input.mousePosition.x <= m_BorderWidth
            ? -m_Distance
            : 0;
        if (Input.GetButton("Jump"))
        {
            mNewPos = m_Player.transform.localPosition;
            mNewPos.z = transform.localPosition.z;
        }
        transform.position = Vector3.Lerp(transform.localPosition, mNewPos, m_Time);
    }
}
