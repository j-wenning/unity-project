using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private Chunk mCurChunk;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Chunk chunk))
        {
            chunk.LoadChunks();
            if (mCurChunk) mCurChunk.UnloadChunks(chunk);
            mCurChunk = chunk;
        }
    }
}
