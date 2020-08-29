using UnityEngine;

public class GridInit : MonoBehaviour
{
    [SerializeField]
    private GameObject m_StarterChunk;

    private GameObject mChunk;

    void Start()
    {
        // eventually set this up to read from the players last chunk theyve been on
        mChunk = Instantiate(m_StarterChunk, transform);
        mChunk.name = mChunk.name.Replace("(Clone)", "");
    }
}
