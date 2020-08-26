using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float m_MoveSpeed = 0.05f;

    private Vector3 mNewPos;

    void Start()
    {
        mNewPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            mNewPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mNewPos.z = transform.position.z;
        }
        transform.position = Vector3.MoveTowards(transform.position, mNewPos, m_MoveSpeed);
    }
}
