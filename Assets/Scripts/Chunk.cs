using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Chunk : MonoBehaviour
{
    public List<GameObject> m_AdjChunks = new List<GameObject>();

    private GameObject mNewChunk;
    private Vector3 mUnloadDistance = new Vector3();

    private void Awake()
    {
        mUnloadDistance.Set(
            transform.localPosition.x + transform.localScale.x,
            transform.localPosition.y + transform.localScale.y,
            transform.localPosition.z + transform.localScale.z
        );
    }

    public void LoadChunks()
    {
        foreach (GameObject chunk in m_AdjChunks)
        {
            if (!GameObject.Find($"/{chunk.name}"))
            {
                mNewChunk = Instantiate(chunk);
                mNewChunk.name = mNewChunk.name.Replace("(Clone)", "");
                mNewChunk = null;
            }
        }
    }
    
    public void UnloadChunks(Chunk newChunk)
    {
        mNewChunk = newChunk.gameObject;
        foreach (GameObject chunk in m_AdjChunks.Except(newChunk.m_AdjChunks).Where(ExcludeCurrent))
        {
            Destroy(GameObject.Find($"/{chunk.name}"));
        }
        mNewChunk = null;
    }

    private bool ExcludeCurrent(GameObject chunk)
    {
        return chunk.name != mNewChunk.name;
    }
}
