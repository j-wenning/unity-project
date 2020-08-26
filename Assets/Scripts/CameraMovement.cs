using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Player;

    public float m_Speed;

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
            ? m_Speed
            : Input.mousePosition.y <= 0
            ? -m_Speed
            : 0;
        mNewPos.x += Input.mousePosition.x >= Screen.width
            ? m_Speed
            : Input.mousePosition.x <= 0
            ? -m_Speed
            : 0;
        if (Input.GetButton("Jump"))
        {
            mNewPos = m_Player.transform.position;
            mNewPos.z = transform.position.z;
        }
        transform.position = mNewPos;
    }
}
