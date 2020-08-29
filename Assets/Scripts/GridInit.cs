using UnityEngine;

public class GridInit : MonoBehaviour
{
    [SerializeField]
    private GameObject m_StarterChunk;

    void Start()
    {
        // eventually set this up to read from the players last chunk theyve been on
        Instantiate(m_StarterChunk, transform);
    }
}
