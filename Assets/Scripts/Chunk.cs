using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public List<GameObject> m_AdjChunks = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LoadChunks();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UnloadChunks();
        }
    }

    private void LoadChunks() {
        foreach (GameObject chunk in m_AdjChunks)
        {
            if (!GameObject.Find($"/Grid/{chunk.name}(Clone)"))
            {
                Instantiate(chunk, transform.parent);
            }
        }
    }
    
    private void UnloadChunks() {
        
    }
}
