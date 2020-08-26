using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Player;

    [SerializeField]
    private float m_Distance = 0.05f;
    [SerializeField]
    private float m_Time = 0.1f;

    private Vector3 mNewPos;

    // Start is called before the first frame update
    void Start()
    {
        mNewPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mNewPos.y += Input.mousePosition.y >= Screen.height
            ? m_Distance
            : Input.mousePosition.y <= 0
            ? -m_Distance
            : 0;
        mNewPos.x += Input.mousePosition.x >= Screen.width
            ? m_Distance
            : Input.mousePosition.x <= 0
            ? -m_Distance
            : 0;
        if (Input.GetButton("Jump"))
        {
            mNewPos.x = m_Player.transform.position.x;
            mNewPos.y = m_Player.transform.position.y;
        }
        transform.position = Vector3.Lerp(transform.position, mNewPos, m_Time);
    }
}
